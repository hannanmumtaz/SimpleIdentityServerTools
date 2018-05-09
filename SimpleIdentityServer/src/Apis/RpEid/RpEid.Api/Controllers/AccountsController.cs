using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpEid.Api.Aggregates;
using RpEid.Api.DTOs.Parameters;
using RpEid.Api.DTOs.Responses;
using RpEid.Api.Extensions;
using RpEid.Api.Parameters;
using RpEid.Api.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Mail;

namespace RpEid.Api.Controllers
{
    [Route(Constants.RouteNames.Accounts)]
    public class AccountsController : Controller
    {
        private const string _confirmAccount = "http://localhost:60005/confirm";
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet(".grant/{id}")]
        [Authorize("IsAdministrator")]
        public async Task<IActionResult> Grant(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var account = await _accountRepository.Get(id);
            if (account == null)
            {
                return new NotFoundResult();
            }

            if (account.IsGranted)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "Grant access has already been given").GetJson());
            }

            if (account.IsConfirmed)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "The account is already confirmed").GetJson());
            }

            account.IsGranted = true;
            account.IsConfirmed = false;
            account.GrantDateTime = DateTime.UtcNow;
            account.ConfirmationCode = Guid.NewGuid().ToString();
            if (!await _accountRepository.Update(account))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "Access cannot be granted").GetJson());
            }

            SendEmail(account.Email, account.ConfirmationCode);
            return new OkResult();
        }

        [HttpGet(".grant/resend/{id}")]
        [Authorize("IsAdministrator")]
        public async Task<IActionResult> Resend(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var account = await _accountRepository.Get(id);
            if (account == null)
            {
                return new NotFoundResult();
            }

            if (!account.IsGranted)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "Grant access has not been given").GetJson());
            }

            if (account.IsConfirmed)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "The account is already confirmed").GetJson());
            }

            account.GrantDateTime = DateTime.UtcNow;
            account.ConfirmationCode = Guid.NewGuid().ToString();
            if (!await _accountRepository.Update(account))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "Access cannot be granted").GetJson());
            }

            SendEmail(account.Email, account.ConfirmationCode);
            return new OkResult();
        }

        [HttpGet(".confirm/{id}")]
        [Authorize("IsConnected")]
        public async Task<IActionResult> Confirm(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var claim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            var sub = claim.Value;
            var accounts = await _accountRepository.Search(new SearchAccountsParameter
            {
                Subjects = new[] { sub },
                ConfirmationCodes = new[] { id }
            });

            if (accounts == null || accounts == null || accounts.Content == null || !accounts.Content.Any())
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "The code is not valid").GetJson());
            }

            var firstAccount = accounts.Content.First();
            if (firstAccount.GrantDateTime >= DateTime.UtcNow.AddDays(2))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "The code is not valid anymore").GetJson());
            }

            firstAccount.IsConfirmed = true;
            firstAccount.ConfirmationDateTime = DateTime.UtcNow;
            if (!await _accountRepository.Update(firstAccount))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "Code cannot be confirmed").GetJson());
            }

            return new OkResult();
        }

        [HttpPost(".search")]
        [Authorize("IsAdministrator")]
        public async Task<IActionResult> Search([FromBody] SearchAccountsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var searchResult = await _accountRepository.Search(request.ToParameter());
            return new OkObjectResult(searchResult);
        }

        [HttpGet(".me")]
        [Authorize("IsConnected")]
        public async Task<IActionResult> Get()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            var sub = claim.Value;
            var account = await _accountRepository.Get(sub);
            if (account == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(account.ToDto());
        }

        [HttpPost]
        [Authorize("IsConnected")]
        public async Task<IActionResult> Add([FromBody] AddAccountParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var claimSub = User.Claims.FirstOrDefault(c => c.Type == "sub");
            var claimName = User.Claims.FirstOrDefault(c => c.Type == "given_name");
            var newAccount = new AccountAggregate
            {
                Email = parameter.Email,
                IsConfirmed = false,
                IsGranted = false,
                Subject = claimSub.Value,
                ConfirmationCode = null
            };
            if (claimName != null)
            {
                newAccount.Name = claimName.Value;
            }

            if ((await _accountRepository.Get(newAccount.Subject)) != null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "An account is already added").GetJson());
            }

            if (!await _accountRepository.Add(newAccount))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "Account cannot be added").GetJson());
            }

            return new OkResult();
        }

        private static void SendEmail(string email, string confirmationCode)
        {
            // WebRequest.DefaultWebProxy = new WebProxy("proxy-legacy.riziv.org", 8080);
            var mail = new MailMessage("szv-noreply@riziv.org", email);
            var client = new SmtpClient
            {
                Port = 25,
                Host = "smtprelay-sp01.riziv.dcs",
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            mail.Subject = "Confirm your account";
            mail.Body = $"Please confirm your account by clicking on the following link : {_confirmAccount}/{confirmationCode}";
            client.Send(mail);
        }
    }
}
