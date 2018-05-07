using Microsoft.AspNetCore.Mvc;
using RpEid.Api.DTOs.Parameters;
using RpEid.Api.DTOs.Responses;
using RpEid.Api.Extensions;
using RpEid.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace RpEid.Api.Controllers
{
    public class AccountsController
    {
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
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

            return new OkObjectResult(account.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AccountResponse account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (!await _accountRepository.Add(account.ToParameter()))
            {
                return null;
            }

            return new OkResult();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccountResponse account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (!await _accountRepository.Add(account.ToParameter()))
            {
                return null;
            }

            return new OkResult();
        }

        [HttpPost(".search")]
        public async Task<IActionResult> Search([FromBody] SearchAccountsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var searchResult = await _accountRepository.Search(request.ToParameter());
            return new OkResult();
        }
    }
}
