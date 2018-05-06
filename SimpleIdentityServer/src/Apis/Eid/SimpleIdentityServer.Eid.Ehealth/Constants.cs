namespace SimpleIdentityServer.Eid.Ehealth
{
    public class Constants
    {
        #region Enumeration values

        public static class KmehrIdentifiers
        {
            public const string IDKMEHR = "ID-KMEHR";
            public const string IDFOLDER = "ID-FOLDER";
            public const string IDTRANSACTION = "ID-TRANSACTION";
            public const string IDHEADING = "ID-HEADING";
            public const string IDITEM = "ID-ITEM";
            public const string IDHCPARTY = "ID-HCPARTY";
            public const string IDPATIENT = "ID-PATIENT";
            public const string LOCAL = "LOCAL";
        }

        public static class KmehrSenderQualifications
        {
            public const string CDHCPARTY = "CD-HCPARTY";
            public const string CDVMPGROUP = "CD-VMPGROUP";
            public const string CDSTANDARD = "CD-STANDARD";
            public const string CDSEX = "CD-SEX";
            public const string CDTRANSACTION = "CD-TRANSACTION";
            public const string CDFEDCOUNTRY = "CD-FED-COUNTRY";
            public const string CDADDRESS = "CD-ADDRESS";
            public const string CDHEADING = "CD-HEADING";
            public const string CDUNIT = "CD-UNIT";
            public const string CDPERIODICITY = "CD-PERIODICITY";
            public const string CDCONTENT = "CD-CONTENT";
            public const string CDDRUGCNK = "CD-DRUG-CNK";
        }

        public static class ErrorCodes
        {
            public const string NotInit = "not_init";
            public const string NoSerialNumber = "no_serialnumber";
            public const string NoAssertion = "no_assertion";
            public const string NoPublicKey = "no_publickey";
        }

        public static class HttpHeaderParameters
        {
            public const string SoapAction = "SOAPAction";
        }

        #endregion

        #region XML definitions

        public static class KmehrCommonNames
        {
            public const string KmehrMessage = "kmehrmessage";
            public const string Id = "id";
            public const string Cd = "cd";
            public const string Content = "content";
            public const string Country = "country";
            public const string Zip = "zip";
            public const string City = "city";
            public const string Street = "street";
            public const string HouseNumber = "housenumber";
            public const string Date = "date";
            public const string Time = "time";
            public const string Author = "author";
            public const string IsComplete = "iscomplete";
            public const string IsValidated = "isvalidated";
            public const string ExpirationDate = "expirationdate";
            public const string Heading = "heading";
            public const string Item = "item";
            public const string FirstName = "firstname";
            public const string FamilyName = "familyname";
            public const string BirthDate = "birthdate";
            public const string BirthLocation = "birthlocation";
            public const string DeathDate = "deathdate";
            public const string DeathLocation = "deathlocation";
            public const string Sex = "sex";
            public const string Nationality = "nationality";
            public const string Patient = "patient";
            public const string Transaction = "transaction";
            public const string TelecomNumber = "telecomnumber";
            public const string HcParty = "hcparty";
            public const string Standard = "standard";
            public const string Sender = "sender";
            public const string Recipient = "recipient";
            public const string Name = "name";
            public const string Telecom = "telecom";
            public const string Firstname = "firstname";
            public const string Lastname = "lastname";
            public const string Address = "address";
            public const string BeginMoment = "beginmoment";
            public const string EndMoment = "endmoment";
            public const string IsRevelant = "isrevelant";
            public const string Quantity = "quantity";
            public const string Frequency = "frequency";
            public const string S = "S";
            public const string Sv = "SV";
            public const string Sl = "SL";
            public const string Header = "header";
            public const string Folder = "folder";
            public const string Decimal = "decimal";
            public const string Unit = "unit";
            public const string UnsignedInt = "unsignedInt";
            public const string Medicinalproduct = "medicinalproduct";
            public const string Intendedcd = "intendedcd";
            public const string Deliveredcd = "deliveredcd";
            public const string Intendedname = "intendedname";
            public const string Deliveredname = "deliveredname";
        }

        public static class EhealthStsNames
        {
            public const string NameIdentifierFormat = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";
            public const string SubjectConfirmationMethod = "urn:oasis:names:tc:SAML:1.0:cm:holder-of-key";
            public const string SsinCertHolderAttributeName = "urn:be:fgov:ehealth:1.0:certificateholder:person:ssin";
            public const string SsinAttributeNamespace = "urn:be:fgov:identification-namespace";
            public const string SsinAttributeName = "urn:be:fgov:person:ssin";
            public const string NameAttributeName = "urn:be:fgov:person:name";
            public const string FirstNameAttributeName = "urn:be:fgov:person:firstName";
            public const string MiddleNameAttributeName = "urn:be:fgov:person:middleName";
            public const string NationalityAttributeName = "urn:be:fgov:person:nationality";
            public const string GenderAttributeName = "urn:be:fgov:person:gender";
            public const string StreetAndNumberAttributeName = "urn:be:fgov:address:streetAndNumber";
            public const string ZipAttributeName = "urn:be:fgov:address:zip";
            public const string MunicipalityAttributeName = "urn:be:fgov:address:municipality";
            public const string CertifiedAttributeNamespace = "urn:be:fgov:certified-namespace:ehealth";
        }

        public static class EhealthXmlAttributeNames
        {
            public const string Lang = "lang";
        }

        public static class EhealthXmlElementNames
        {
            public const string PhaseCode = "PhaseCode";
            public const string InscriptionCode = "InscriptionCode";
            public const string Active = "ACTIVE";
            public const string Suspended = "SUSPENDED";
            public const string Revoked = "REVOKED";
            public const string ReimbursementCode = "ReimbursementCode";
            public const string CrmLink = "CrmLink";
            public const string PosologyNote = "PosologyNote";
            public const string Note = "Note";
            public const string PrescriptionName = "PrescriptionName";
            public const string AbbreviatedName = "AbbreviatedName";
            public const string RmaProfessionalLink = "RmaProfessionalLink";
            public const string RmaPatientLink = "RmaPatientLink";
            public const string SpcLink = "SpcLink";
            public const string LeafletLink = "LeafletLink";
            public const string Text = "Text";
            public const string Dmpp = "Dmpp";
            public const string Code = "Code";
            public const string CodeType = "CodeType";
            public const string Message = "Message";
            public const string Type = "Type";
            public const string Value = "Value";
            public const string ApplicationId = "ApplicationID";
            public const string Identifier = "Identifier";
            public const string SearchCriteria = "SearchCriteria";
            public const string GetEtkRequest = "GetEtkRequest";
            public const string Organisation = "Organisation";
            public const string CreatePrescriptionRequest = "CreatePrescriptionRequest";
            public const string SecuredContent = "SecuredContent";
            public const string GetNewKeyRequest = "GetNewKeyRequest";
            public const string SecuredCreatePrescriptionRequest = "SecuredCreatePrescriptionRequest";
            public const string AdministrativeInformation = "AdministrativeInformation";
            public const string PatientIdentifier = "PatientIdentifier";
            public const string PrescriptionType = "PrescriptionType";
            public const string KeyIdentifier = "KeyIdentifier";
            public const string SealedContent = "SealedContent";
            public const string GetNewKeyRequestContent = "GetNewKeyRequestContent";
            public const string SealedNewKeyResponse = "SealedNewKeyResponse";
            public const string FindCompoundingIngredientRequest = "FindCompoundingIngredientRequest";
            public const string FindByOfficialName = "FindByOfficialName";
            public const string FindByAnyName = "FindByAnyName";
            public const string PersonData = "PersonData";
            public const string FindByCnk = "FindByCNK";
            public const string Inscription = "Inscription";
            public const string SearchBySSINRequest = "SearchBySSINRequest";
            public const string Period = "Period";
            public const string Etk = "ETK";
            public const string CBE = "CBE";
            public const string SSIN = "SSIN";
            public const string OrgUnit = "OrgUnit";
            public const string NIHII = "NIHII";
            public const string NIHII_PHARMACY = "NIHII_PHARMACY";
            public const string First = "First";
            public const string Middle = "Middle";
            public const string Last = "Last";
            public const string Name = "Name";
            public const string QualityCode = "QualityCode";
            public const string BeginDate = "BeginDate";
            public const string EndDate = "EndDate";
            public const string Purpose = "Purpose";
            public const string SearchBySSINReply = "SearchBySSINReply";
            public const string Status = "Status";
            public const string Gender = "Gender";
            public const string Person = "Person";
            public const string FindAmpRequest = "FindAmpRequest";
            public const string FindByProduct = "FindByProduct";
            public const string AmpCode = "AmpCode";
            public const string AnyNamePart = "AnyNamePart";
            public const string FindAmpResponse = "FindAmpResponse";
            public const string Amp = "Amp";
            public const string Public = "P";
            public const string Ambulatory = "A";
            public const string Hospital = "H";
            public const string ResidentialCare = "R";
            public const string DeliveryEnvironment = "DeliveryEnvironment";
            public const string Cnk = "CNK";
            public const string Pseudo = "PSEUDO";
            public const string Ampp = "Ampp";
            public const string BlackTriangle = "BlackTriangle";
            public const string OfficialName = "OfficialName";
            public const string Orphan = "Orphan";
            public const string SingleUse = "SingleUse";
            public const string AuthorisationNr = "AuthorisationNr";
            public const string PackDisplayValue = "PackDisplayValue";
            public const string ExFactoryPrice = "ExFactoryPrice";
        }

        public static class EhealthNamespaces
        {
            public const string EhealthCommon = "urn:be:fgov:ehealth:commons:1_0:core";
            public const string GetEtkRequest = "urn:be:fgov:ehealth:etkdepot:1_0:protocol";
            public const string RecipeNamespace = "urn:be:fgov:ehealth:recipe:protocol:v1";
            public const string KgssNamespace = "urn:be:fgov:ehealth:etee:kgss:1_0:protocol";
            public const string ConsultRn = "urn:be:fgov:ehealth:consultRN:1_0:protocol";
            public const string Dics = "urn:be:fgov:ehealth:dics:protocol:v2";
            public const string Kmehr = "http://www.ehealth.fgov.be/standards/kmehr/schema/v1";
        }

        public static class EhealthPrefixes
        {
            public const string GetEtkResponse = "ns4";
            public const string Ns1 = "ns1";
        }

        #endregion
    }
}
