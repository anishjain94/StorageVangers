using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;
using static Google.Apis.Drive.v3.Data.About;

namespace StorageVangers.Api.Services
{
    public class StorageService : IStorageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StorageService(IHttpContextAccessor httpContextAccessor) 
            => (_httpContextAccessor) = (httpContextAccessor);

        public async Task<StorageQuotaData> GetGoogleDriveInfoAsync()
        {
            if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("GoogleAccessToken", out StringValues accessToken))
            {
                throw new Exception("Access Token Not Found.");
            }

            var googleCreds = GoogleCredential.FromAccessToken(accessToken);
            var driveService = new DriveService(new BaseClientService.Initializer { HttpClientInitializer = googleCreds });
            var aboutRequest = driveService.About.Get();
            aboutRequest.Fields = "storageQuota(limit,usage,usageInDrive,usageInDriveTrash)";
            var aboutResponse = await aboutRequest.ExecuteAsync();
            return aboutResponse.StorageQuota;
        }
    }
}
