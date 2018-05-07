using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RpEid.Api.Aggregates;
using RpEid.Api.Models;
using RpEid.Api.Parameters;
using RpEid.Api.Results;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RpEid.Api.Repositories
{
    public interface IAccountRepository
    {
        Task<SearchAccountsResult> Search(SearchAccountsParameter parameter);
        Task<AccountAggregate> Get(string subject);
        Task<bool> Add(AccountAggregate account);
        Task<bool> Update(AccountAggregate account);
    }

    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public AccountRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<SearchAccountsResult> Search(SearchAccountsParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<RpEidContext>())
                {
                    IQueryable<Account> accounts = context.Accounts;
                    if (parameter.Subjects != null)
                    {
                        accounts = accounts.Where(a => parameter.Subjects.Contains(a.Subject));
                    }

                    switch (parameter.Order)
                    {
                        case Orders.Asc:
                            accounts = accounts.OrderBy(a => a.UpdateDateTime);
                            break;
                        case Orders.Desc:
                            accounts = accounts.OrderByDescending(a => a.UpdateDateTime);
                            break;
                    }

                    var nbResult = await accounts.CountAsync().ConfigureAwait(false);
                    if (parameter.IsPagingEnabled)
                    {
                        accounts = accounts.Skip(parameter.StartIndex).Take(parameter.Count);
                    }

                    return new SearchAccountsResult
                    {
                        Content = await accounts.Select(a => GetAccount(a)).ToListAsync().ConfigureAwait(false),
                        StartIndex = parameter.StartIndex,
                        TotalResults = nbResult
                    };
                }
            }
        }

        public async Task<AccountAggregate> Get(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }
            
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<RpEidContext>())
                {
                    var account = await context.Accounts.FirstOrDefaultAsync(a => a.Subject == subject);
                    if (account == null)
                    {
                        return null;
                    }

                    return GetAccount(account);
                }
            }
        }

        public async Task<bool> Add(AccountAggregate account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<RpEidContext>())
                {
                    var record = new Account
                    {
                        CreateDateTime = DateTime.UtcNow,
                        UpdateDateTime = DateTime.UtcNow,
                        IsConfirmed = false,
                        Name = account.Name,
                        Subject = account.Subject
                    };

                    context.Accounts.Add(record);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    return true;
                }
            }
        }

        public async Task<bool> Update(AccountAggregate account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<RpEidContext>())
                {
                    var record = await context.Accounts.FirstOrDefaultAsync(a => a.Subject == account.Subject);
                    if (record == null)
                    {
                        return false;
                    }

                    record.UpdateDateTime = DateTime.UtcNow;
                    record.Name = account.Name;
                    record.Email = account.Email;
                    record.IsConfirmed = account.IsConfirmed;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    return true;
                }
            }
        }

        private static AccountAggregate GetAccount(Account account)
        {
            return new AccountAggregate
            {
                IsConfirmed = account.IsConfirmed,
                Name = account.Name,
                Subject = account.Subject,
                Email = account.Email,
                UpdateDateTime = account.UpdateDateTime,
                CreateDateTime = account.CreateDateTime
            };
        }
    }
}
