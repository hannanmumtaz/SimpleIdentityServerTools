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
                IsConfirmed = request.IsConfirmed,
                IsPagingEnabled = true,
                Order = (Orders)request.Order,
                StartIndex = request.StartIndex,
                Subjects = request.Subjects
            };
        }

        public static AccountAggregate ToParameter(this AccountResponse account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            return new AccountAggregate
            {
                CreateDateTime = account.CreateDateTime,
                Email = account.Email,
                IsConfirmed = account.IsConfirmed,
                Name = account.Name,
                Subject = account.Subject,
                UpdateDateTime = account.UpdateDateTime
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
                CreateDateTime = account.CreateDateTime,
                Email = account.Email,
                IsConfirmed = account.IsConfirmed,
                Name = account.Name,
                Subject = account.Subject,
                UpdateDateTime = account.UpdateDateTime
            };
        }
    }
}
