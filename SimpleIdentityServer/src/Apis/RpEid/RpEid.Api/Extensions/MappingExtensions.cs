using RpEid.Api.Aggregates;
using RpEid.Api.DTOs.Parameters;
using RpEid.Api.DTOs.Responses;
using RpEid.Api.Parameters;
using System;

namespace RpEid.Api.Extensions
{
    internal static class MappingExtensions
    {
        public static SearchAccountsParameter ToParameter(this SearchAccountsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new SearchAccountsParameter
            {
                Count = request.Count,
                IsPagingEnabled = true,
                Order = (Orders)request.Order,
                StartIndex = request.StartIndex,
                Subjects = request.Subjects
            };
        }

        public static AccountResponse ToDto(this AccountAggregate account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            return new AccountResponse
            {
                Subject = account.Subject,
                Name = account.Name,
                Email = account.Email,
                IsGranted = account.IsGranted,
                IsConfirmed = account.IsConfirmed,
                CreateDateTime = account.CreateDateTime,
                UpdateDateTime = account.UpdateDateTime,
                GrantDateTime = account.GrantDateTime,
                ConfirmationDateTime = account.ConfirmationDateTime
            };
        }
    }
}
