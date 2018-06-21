using System;
using System.IO;
using Xunit;

namespace SimpleIdentityServer.License.Tests
{
    public class LicenseGeneratorFixture
    {
        [Fact]
        public void When_Generate_License_Then_File_Is_Generated()
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "Certificates");
            Environment.SetEnvironmentVariable("SID_LICENSE", directory);
            var builder = new LicenseGeneratorBuilder();
            builder.New().SetOrganisation("organisation")
                .SetIssueDateTime(DateTime.UtcNow)
                .SetExpirationDateTime(DateTime.UtcNow.AddDays(30))
                .SetType("commercial")
                .Save();
            string s = "";
        }

        [Fact]
        public void When_Trying_To_GetLicense_File_Then_Informations_Are_Returned()
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "Certificates");
            Environment.SetEnvironmentVariable("SID_LICENSE", directory);
            var loader = new LicenseLoader();
            var license = loader.TryGetLicense();
            var t = "";
        }
    }
}
