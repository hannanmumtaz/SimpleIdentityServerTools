using SimpleIdentityServer.Scim.Db.EF;
using SimpleIdentityServer.Scim.Db.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.ScimProvider.Modularized.Startup.Extensions
{

    public static class ScimDbContextExtensions
    {
        private const char _separator = ',';
        private static string _id = Guid.NewGuid().ToString();
        private static string _externalId = Guid.NewGuid().ToString();
        private static string _adrId = Guid.NewGuid().ToString();
        private static string _localeId = Guid.NewGuid().ToString();
        private static string _ageId = Guid.NewGuid().ToString();
        private static string _genderId = Guid.NewGuid().ToString();
        private static string _ethnicityId = Guid.NewGuid().ToString();
        private static string _birthDateId = Guid.NewGuid().ToString();
        private static string _locationId = Guid.NewGuid().ToString();

        public static void EnsureSeedData(this ScimDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            InsertSchemas(context);
            InsertRepresentations(context);
            try
            {
                context.SaveChanges();
            }
            catch { }
        }

        private static void InsertSchemas(ScimDbContext context)
        {
            if (!context.Schemas.Any())
            {
                context.Schemas.Add(UserSchema);
                context.Schemas.Add(GroupSchema);
                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    // Trace.WriteLine("duplicated keys");
                }
            }
        }

        private static void InsertRepresentations(ScimDbContext context)
        {
            if (!context.Representations.Any())
            {
                context.Representations.AddRange(new[]
                {
                    new Representation
                    {
                        Id = "7d79392f-8a02-494c-949e-723a4db8ed16",
                        Version = "117ee9e4-e519-4ce6-b748-9691f70b43ce",
                        Created = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        ResourceType = SimpleIdentityServer.Scim.Common.Constants.ResourceTypes.User,
                        Attributes = new List<RepresentationAttribute>
                        {
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _externalId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _adrId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _localeId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _ageId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _genderId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _ethnicityId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _birthDateId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _locationId
                            }
                        }
                    },
                    new Representation
                    {
                        Id = "c41c9e28-a4f8-447d-b170-f99563257d15",
                        Version = "b424ab6d-244d-4fa5-b4af-a27a23665996",
                        Created = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        ResourceType = SimpleIdentityServer.Scim.Common.Constants.ResourceTypes.User,
                        Attributes = new List<RepresentationAttribute>
                        {
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _externalId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _adrId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _localeId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _ageId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _genderId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _ethnicityId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _birthDateId
                            },
                            new RepresentationAttribute
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                                SchemaAttributeId = _locationId
                            }
                        }
                    }
                });
            }
        }

        private static class SchemaAttributeFactory
        {
            public static SchemaAttribute CreateAttributeWithId(
                string id,
                string name,
                string description,
                string type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                string mutability = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite,
                string returned = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeReturned.Default,
                string uniqueness = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeUniqueness.None,
                bool caseExact = false,
                bool required = false,
                bool multiValued = false,
                string[] referenceTypes = null,
                string[] canonicalValues = null,
                bool isCommon = false)
            {
                return new SchemaAttribute
                {
                    Id = id,
                    Name = name,
                    Type = type,
                    MultiValued = multiValued,
                    Description = description,
                    Required = required,
                    CaseExact = caseExact,
                    Mutability = mutability,
                    Returned = returned,
                    Uniqueness = uniqueness,
                    ReferenceTypes = ConcatList(referenceTypes),
                    CanonicalValues = ConcatList(canonicalValues),
                    IsCommon = isCommon
                };
            }

            public static SchemaAttribute CreateAttribute(
                string name,
                string description,
                string type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                string mutability = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite,
                string returned = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeReturned.Default,
                string uniqueness = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeUniqueness.None,
                bool caseExact = false,
                bool required = false,
                bool multiValued = false,
                string[] referenceTypes = null,
                string[] canonicalValues = null,
                bool isCommon = false)
            {
                return CreateAttributeWithId(Guid.NewGuid().ToString(), name, description, type, mutability, returned, uniqueness, caseExact, required, multiValued, referenceTypes, canonicalValues, isCommon);
            }

            public static SchemaAttribute CreateComplexAttribute(
                string id,
                string name,
                string description,
                List<SchemaAttribute> subAttributes,
                string type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                bool multiValued = false,
                bool required = false,
                bool caseExact = false,
                string mutability = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite,
                string returned = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeReturned.Default,
                string uniqueness = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeUniqueness.None,
                string[] referenceTypes = null,
                string[] canonicalValues = null,
                bool isCommon = false)
            {
                return new SchemaAttribute
                {
                    Id = id,
                    Name = name,
                    MultiValued = multiValued,
                    Description = description,
                    Required = required,
                    CaseExact = caseExact,
                    Mutability = mutability,
                    Returned = returned,
                    Uniqueness = uniqueness,
                    ReferenceTypes = ConcatList(referenceTypes),
                    CanonicalValues = ConcatList(canonicalValues),
                    Children = subAttributes,
                    IsCommon = isCommon,
                    Type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.Complex
                };
            }

            public static SchemaAttribute CreateValueAttribute(
                string description,
                string[] referenceTypes = null,
                string type = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                string mutability = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite)
            {
                return CreateAttribute(
                        SimpleIdentityServer.Scim.Common.Constants.MultiValueAttributeNames.Value,
                        description,
                        type: type,
                        referenceTypes: referenceTypes,
                        mutability: mutability);
            }

            public static SchemaAttribute CreateDisplayAttribute(
                string description,
                string mutability = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite)
            {
                return CreateAttribute(
                        SimpleIdentityServer.Scim.Common.Constants.MultiValueAttributeNames.Display,
                        description,
                        mutability: mutability);
            }

            public static SchemaAttribute CreateTypeAttribute(
                string description,
                string[] canonicalValues,
                string mutability = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite)
            {
                return CreateAttribute(
                    SimpleIdentityServer.Scim.Common.Constants.MultiValueAttributeNames.Type,
                    description,
                    canonicalValues: canonicalValues,
                    mutability: mutability);
            }

            public static SchemaAttribute CreatePrimaryAttribute(
                string description,
                string mutability = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite)
            {
                return CreateAttribute(
                    SimpleIdentityServer.Scim.Common.Constants.MultiValueAttributeNames.Primary,
                    description,
                    type: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.Boolean,
                    mutability: mutability);
            }

            public static SchemaAttribute CreateRefAttribute(
                string description,
                string[] referenceTypes,
                string mutability = SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite)
            {
                return CreateAttribute(
                    SimpleIdentityServer.Scim.Common.Constants.MultiValueAttributeNames.Ref,
                    description,
                    type: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.Reference,
                    referenceTypes: referenceTypes,
                    mutability: mutability);
            }
        }

        private static List<SchemaAttribute> UserMetaDataAttributes = new List<SchemaAttribute>
        {
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.ResourceType, "Name of the resource type of the resource", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, caseExact: true),
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.Created, "The 'DateTime' that the resource was added to the service provider", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, type: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.DateTime),
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.LastModified, "The most recent DateTime than the details of this resource were updated at the service provider", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, type: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.DateTime),
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.Location, "URI of the resource being returned", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly),
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.Version, "Version of the resource being returned", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, caseExact: true),
        };

        private static List<SchemaAttribute> GroupMetaDataAttributes = new List<SchemaAttribute>
        {
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.ResourceType, "Name of the resource type of the resource", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, caseExact: true),
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.Created, "The 'DateTime' that the resource was added to the service provider", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, type: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.DateTime),
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.LastModified, "The most recent DateTime than the details of this resource were updated at the service provider", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, type: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.DateTime),
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.Location, "URI of the resource being returned", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly),
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.MetaResponseNames.Version, "Version of the resource being returned", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, caseExact: true),
        };

        #region User

        #region Attributes

        private static List<SchemaAttribute> UserAddressAttributes = new List<SchemaAttribute>
        {
             SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.AddressResponseNames.Formatted, "The full mailing address, formatted for display or use with a mailing label.  This attribute MAY contain newlines."),
             SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.AddressResponseNames.StreetAddress, "The full street address component, which may include house number, street name, P.O. box, and multi-line extended street address information.  This attribute MAY contain newlines."),
             SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.AddressResponseNames.Locality, "The city or locality component."),
             SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.AddressResponseNames.Region, "The state or region component."),
             SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.AddressResponseNames.PostalCode, "The zip code or postal code component."),
             SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.AddressResponseNames.Country, "The country name component."),
             SchemaAttributeFactory.CreateTypeAttribute("A label indicating the attribute's function, e.g., 'work' or 'home'.", new string[] { "work", "home", "other" })
        };

        #endregion

        private static Schema UserSchema = new Schema
        {
            Id = SimpleIdentityServer.Scim.Common.Constants.SchemaUrns.User,
            Name = SimpleIdentityServer.Scim.Common.Constants.ResourceTypes.User,
            Description = "User Account",
            Attributes = new List<SchemaAttribute>
            {
                // locale
                SchemaAttributeFactory.CreateAttributeWithId(
                    _localeId,
                     SimpleIdentityServer.Scim.Common.Constants.UserResourceResponseNames.Locale,
                     "Used to indicate the User's default location"+
                                    "for purposes of localizing items such as currency, date time format, or"+
                                    "numerical representations."),
                // Age
                SchemaAttributeFactory.CreateAttributeWithId(
                    _ageId,
                     "age",
                     "Age of the user",
                     type: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.Integer),
                // Gender
                SchemaAttributeFactory.CreateAttributeWithId(
                    _genderId,
                     "gender",
                     "Possible values (male, female, unknown)",
                     type: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeTypes.String,
                     canonicalValues: new string[] { "male", "female", "unknown" }),
                // Addresses
                SchemaAttributeFactory.CreateComplexAttribute(
                    _adrId,
                    SimpleIdentityServer.Scim.Common.Constants.UserResourceResponseNames.Addresses,
                    "A physical mailing address for this User. Canonical type values of 'work', 'home', and 'other'.  This attribute is a complex type with the following sub-attributes.",
                    UserAddressAttributes,
                    multiValued: true),
                SchemaAttributeFactory.CreateAttributeWithId(_id,
                    SimpleIdentityServer.Scim.Common.Constants.IdentifiedScimResourceNames.Id,
                    "Unique identifier for a SCIM resource as defined by the service provider",
                    mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly,
                    caseExact: true,
                    returned: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeReturned.Always,
                    isCommon: true),
                SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.IdentifiedScimResourceNames.ExternalId, "Identifier as defined by the provisioning client", caseExact: true, mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite, required: false, isCommon: true),
                SchemaAttributeFactory.CreateComplexAttribute(Guid.NewGuid().ToString(), SimpleIdentityServer.Scim.Common.Constants.ScimResourceNames.Meta, "Complex attribute contaning resource metadata", UserMetaDataAttributes, mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, returned: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeReturned.Default, isCommon: true)
            },
            Meta = new MetaData
            {
                Id = Guid.NewGuid().ToString(),
                ResourceType = "Schema",
                Location = SimpleIdentityServer.Scim.Common.Constants.SchemaUrns.User
            }
        };

        #endregion

        #region Group

        private static List<SchemaAttribute> GroupMembersAttribute = new List<SchemaAttribute>
        {
            SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.GroupMembersResponseNames.Value, "Identifier of the member of this Group.", uniqueness: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeUniqueness.None, required : false, mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.Immutable),
            SchemaAttributeFactory.CreateRefAttribute("The URI corresponding to a SCIM resource that is a member of this Group.", new string[] { "User", "Group" }, SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.Immutable),
            SchemaAttributeFactory.CreateTypeAttribute("A label indicating the type of resource, e.g., 'User' or 'Group'.", new string[] { "User", "Group" }, SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.Immutable)
        };

        private static Schema GroupSchema = new Schema
        {
            Id = SimpleIdentityServer.Scim.Common.Constants.SchemaUrns.Group,
            Name = SimpleIdentityServer.Scim.Common.Constants.ResourceTypes.Group,
            Description = "Group",
            Attributes = new List<SchemaAttribute>
            {
                SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.GroupResourceResponseNames.DisplayName, "A human-readable name for the Group. REQUIRED.", uniqueness: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeUniqueness.None, required : false),
                SchemaAttributeFactory.CreateComplexAttribute(Guid.NewGuid().ToString(), SimpleIdentityServer.Scim.Common.Constants.GroupResourceResponseNames.Members, "A list of members of the Group.", GroupMembersAttribute, multiValued: true),
                SchemaAttributeFactory.CreateAttribute(SimpleIdentityServer.Scim.Common.Constants.IdentifiedScimResourceNames.Id, "Unique identifier for a SCIM resource as defined by the service provider", mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, caseExact: true, returned: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeReturned.Always, isCommon: true),
                SchemaAttributeFactory.CreateAttributeWithId(_externalId, SimpleIdentityServer.Scim.Common.Constants.IdentifiedScimResourceNames.ExternalId, "Identifier as defined by the provisioning client", caseExact: true, mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadWrite, required: false, isCommon: true),
                SchemaAttributeFactory.CreateComplexAttribute(Guid.NewGuid().ToString(), SimpleIdentityServer.Scim.Common.Constants.ScimResourceNames.Meta, "Complex attribute contaning resource metadata", GroupMetaDataAttributes, mutability: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeMutability.ReadOnly, returned: SimpleIdentityServer.Scim.Common.Constants.SchemaAttributeReturned.Default, isCommon: true)
            },
            Meta = new MetaData
            {
                Id = Guid.NewGuid().ToString(),
                ResourceType = "Schema",
                Location = SimpleIdentityServer.Scim.Common.Constants.SchemaUrns.Group
            }
        };

        #endregion

        private static string ConcatList(IEnumerable<string> lst)
        {
            if (lst == null)
            {
                return string.Empty;
            }

            return string.Join(_separator.ToString(), lst);
        }
    }
}
