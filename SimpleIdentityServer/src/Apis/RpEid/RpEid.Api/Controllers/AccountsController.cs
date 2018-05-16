using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpEid.Api.Aggregates;
using RpEid.Api.DTOs.Parameters;
using RpEid.Api.DTOs.Responses;
using RpEid.Api.Extensions;
using RpEid.Api.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

        #region Administrator actions

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

            account.IsGranted = true;
            account.GrantDateTime = DateTime.UtcNow;
            if (!await _accountRepository.Update(account))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "Access cannot be granted").GetJson());
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

        #endregion

        #region User actions

        [HttpPost]
        [Authorize("IsConnected")]
        public async Task<IActionResult> Add([FromBody] AddAccountParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var claimSub = User.Claims.FirstOrDefault(c => c.Type == "sub");
            if ((await _accountRepository.Get(claimSub.Value)) != null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "An account has already been added").GetJson());
            }

            SendEmail(claimSub.Value, parameter.Email);
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

            var claimSub = User.Claims.FirstOrDefault(c => c.Type == "sub");
            string email = null;
            if (!CheckCode(claimSub.Value, id, out email))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "The code is not valid").GetJson());
            }

            var claimName = User.Claims.FirstOrDefault(c => c.Type == "given_name");
            var newAccount = new AccountAggregate
            {
                Email = email,
                IsGranted = false,
                Subject = claimSub.Value,
                Name = claimName.Value
            };

            if ((await _accountRepository.Get(newAccount.Subject)) != null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "An account has already been added").GetJson());
            }

            if (!await _accountRepository.Add(newAccount))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse(Constants.ErrorCodes.InvalidRequest, "Account cannot be added").GetJson());
            }

            return new OkResult();
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

        #endregion

        private static void SendEmail(string subject, string email)
        {
            var confirmationCode = GenerateCode(subject, email);
            var mail = new MailMessage("Thierry.Habart@inami.fgov.be", email);
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

        private static string GenerateCode(string subject, string email)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            var confirmationCode = Guid.NewGuid().ToString();
            var expirationTime = ToUnixTimestamp(DateTime.UtcNow.AddDays(2));
            var conc = $"{subject}.{email}.{expirationTime}.{confirmationCode}";
            byte[] hashPayload = null;
            using (var sha = SHA1.Create())
            {
                var payload = Encoding.UTF8.GetBytes(conc);
                hashPayload = sha.ComputeHash(payload);
            }

            var certificate = new X509Certificate2("certificate.pfx");
            var privateKey = (RSACryptoServiceProvider)certificate.PrivateKey;
            var signature = privateKey.SignHash(hashPayload, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{confirmationCode} {email} {expirationTime} {Convert.ToBase64String(signature)}"));
        }

        private static bool CheckCode(string subject, string code, out string mail)
        {
            mail = null;
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            var splittedCode = code.Split(' ');
            if (splittedCode.Length != 4)
            {
                return false;
            }

            var confirmationCode = splittedCode[0];
            var email = splittedCode[1];
            var expirationTime = splittedCode[2];
            var signature = splittedCode[3];
            var expirationDateTime = ToDateTime(double.Parse(expirationTime));
            if (DateTime.UtcNow > expirationDateTime)
            {
                return false;
            }

            var conc = $"{subject}.{email}.{expirationTime}.{confirmationCode}";
            var signaturePayload = Convert.FromBase64String(signature);
            byte[] hashPayload = null;
            using (var sha = SHA1.Create())
            {
                var payload = Encoding.UTF8.GetBytes(conc);
                hashPayload = sha.ComputeHash(payload);
            }

            var certificate = new X509Certificate2("certificate.pfx");
            var publicKey = (RSACryptoServiceProvider)certificate.PublicKey.Key;
            mail = email;
            return publicKey.VerifyHash(hashPayload, signaturePayload, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        private static int ToUnixTimestamp(DateTime value)
        {
            return (int)Math.Truncate((value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        public static DateTime ToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
