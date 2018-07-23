using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.EF;
using SimpleIdentityServer.EF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleIdentityServer.Manager.Host.Tests
{
    public static class SimpleIdentityServerContextExtensions
    {
        public static void EnsureSeedData(this SimpleIdentityServerContext context, SharedContext sharedCtx)
        {
            InsertClaims(context);
            InsertScopes(context);
            InsertResourceOwners(context);
            try
            {
                context.SaveChanges();
            }
            catch
            {
                Trace.WriteLine("items already exists");
            }
        }

        private static void InsertClaims(SimpleIdentityServerContext context)
        {
            if (!context.Claims.Any())
            {
                context.Claims.AddRange(new[] {
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject, IsIdentifier = true },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Name },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.FamilyName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.MiddleName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.NickName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PreferredUserName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Profile },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Picture },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.WebSite },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Gender },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.BirthDate },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ZoneInfo },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Locale },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.UpdatedAt },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Email },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.EmailVerified },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Address },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumber },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumberVerified },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimId },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimLocation }
                });
            }
        }

        private static void InsertScopes(SimpleIdentityServerContext context)
        {
            if (!context.Scopes.Any())
            {
                context.Scopes.AddRange(new[] {
                    new Scope
                    {
                        Name = "openid",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        IsDisplayedInConsent = true,
                        Description = "access to the openid scope",
                        Type = ScopeType.ProtectedApi
                    },
                    new Scope
                    {
                        Name = "profile",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        Description = "Access to the profile",
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Name },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.FamilyName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.MiddleName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.NickName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PreferredUserName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Profile },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Picture },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.WebSite },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Gender },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.BirthDate },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ZoneInfo },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Locale },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.UpdatedAt }
                        },
                        Type = ScopeType.ResourceOwner,
                        IsDisplayedInConsent = true
                    },
                    new Scope
                    {
                        Name = "scim",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        Description = "Access to the scim",
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimId },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimLocation }
                        },
                        Type = ScopeType.ResourceOwner,
                        IsDisplayedInConsent = true
                    },
                    new Scope
                    {
                        Name = "email",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        IsDisplayedInConsent = true,
                        Description = "Access to the email",
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Email },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.EmailVerified }
                        },
                        Type = ScopeType.ResourceOwner
                    },
                    new Scope
                    {
                        Name = "address",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        IsDisplayedInConsent = true,
                        Description = "Access to the address",
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Address }
                        },
                        Type = ScopeType.ResourceOwner
                    },
                    new Scope
                    {
                        Name = "phone",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        IsDisplayedInConsent = true,
                        Description = "Access to the phone",
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumber },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumberVerified }
                        },
                        Type = ScopeType.ResourceOwner
                    },
                    new Scope
                    {
                        Name = "role",
                        IsExposed = true,
                        IsOpenIdScope = false,
                        IsDisplayedInConsent = true,
                        Description = "Access to your roles",
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role }
                        },
                        Type = ScopeType.ResourceOwner
                    },
                    new Scope
                    {
                        Name = "api1",
                        IsExposed = false,
                        IsOpenIdScope = false,
                        IsDisplayedInConsent = true,
                        Description = "Access to your api1",
                        Type = ScopeType.ProtectedApi
                    },
                    new Scope
                    {
                        Name = "register_client",
                        IsExposed = false,
                        IsOpenIdScope = false,
                        IsDisplayedInConsent = true,
                        Description = "Register a client",
                        Type = ScopeType.ProtectedApi
                    }
                });
            }
        }

        private static void InsertResourceOwners(SimpleIdentityServerContext context)
        {
            if (!context.ResourceOwners.Any())
            {
                context.ResourceOwners.AddRange(new[]
                {
                    new ResourceOwner
                    {
                        Id = "administrator",
                        Claims = new List<ResourceOwnerClaim>
                        {
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject,                                
                                Value = "administrator"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role,
                                Value = "administrator"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Address,
                                Value = "{ country : 'france' }"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.BirthDate,
                                Value = "1989-10-07"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Email,
                                Value = "habarthierry@hotmail.fr"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.EmailVerified,
                                Value = "true"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.FamilyName,
                                Value = "habart"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Gender,
                                Value = "M"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName,
                                Value = "Habart Thierry"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Locale,
                                Value = "fr-FR"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.MiddleName,
                                Value = "Thierry"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode =SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.NickName,
                                Value = "Titi"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumber,
                                Value = "+32485350536"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumberVerified,
                                Value = "true"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Picture,
                                Value = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/58/Shiba_inu_taiki.jpg/220px-Shiba_inu_taiki.jpg"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PreferredUserName,
                                Value = "Thierry"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Profile,
                                Value = "http://localhost/profile"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.UpdatedAt,
                                Value = DateTime.Now.ConvertToUnixTimestamp().ToString()
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.WebSite,
                                Value = "https://github.com/thabart"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ZoneInfo,
                                Value = "Europe/Paris"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimId,
                                Value = "id"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimLocation,
                                Value = "http://localhost:5555/Users/id"
                            }
                        },
                        Password = "password",
                        IsLocalAccount = true
                    },
                    new ResourceOwner
                    {
                        Id = "user",
                        Password = "password",
                        Claims = new List<ResourceOwnerClaim>
                        {
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject,
                                Value = "user"
                            }
                        },
                        IsLocalAccount = true
                    }
                });
            }
        }
    }
}
