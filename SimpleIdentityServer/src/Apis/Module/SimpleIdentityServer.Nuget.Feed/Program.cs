using Microsoft.Owin.Hosting;
using NuGet.Server.Core.Infrastructure;
using NuGet.Server.Core.Logging;
using NuGet.Server.V2;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Nuget.Feed
{
    public class Program
    {
        public static IServerPackageRepository NuGetPrivateRepository { get; private set; }
        public static IServerPackageRepository NuGetPublicRepository { get; private set; }
        public static IServerPackageRepository NuGetVeryPublicRepository { get; private set; }
        public const string ApiKey = "key123";

        static void Main(string[] args)
        {
            var baseAddress = "http://localhost:60011/";
            var settings = new Dictionary<string, object>();
            settings.Add("enableDelisting", false);
            settings.Add("enableFrameworkFiltering", false);
            settings.Add("ignoreSymbolsPackages", true);
            settings.Add("allowOverrideExistingPackageOnPush", true);
            var settingsProvider = new DictionarySettingsProvider(settings);
            var logger = new ConsoleLogger();
            NuGetPrivateRepository = NuGetV2WebApiEnabler.CreatePackageRepository(@"d:\omnishopcentraldata\Packages\Private", settingsProvider, logger);
            NuGetPublicRepository = NuGetV2WebApiEnabler.CreatePackageRepository(@"d:\omnishopcentraldata\Packages\Public", settingsProvider, logger);
            NuGetVeryPublicRepository = NuGetV2WebApiEnabler.CreatePackageRepository(@"d:\omnishopcentraldata\Packages\VeryPublic", settingsProvider, logger);
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Server listening at baseaddress: " + baseAddress);
                Console.WriteLine("[ENTER] to close server");
                Console.ReadLine();
            }
        }
    }
}