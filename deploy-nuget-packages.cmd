REM DEPLOY TO THE PRIVATE FEED
nuget push *.nupkg Kx8yuzyc7J4n2gdCdaTLEX88u -Source http://localhost:52594/nuget
REM DEPLOY TO THE PUBLIC FEED
nuget push *.nupkg e3c68d21-0ce4-4910-ba91-7278b221b6a9 -source https://www.myget.org/F/advance-ict/api/v3/index.json