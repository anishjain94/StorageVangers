using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace StorageVangers.Api.Services
{
    public class StorageService : IStorageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DriveService _driveService;

        public StorageService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;

            if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("GoogleAccessToken", out StringValues accessToken))
            {
                throw new Exception("Access Token Not Found.");
            }

            var googleCreds = GoogleCredential.FromAccessToken(accessToken);
            _driveService = new DriveService(new BaseClientService.Initializer { HttpClientInitializer = googleCreds });
        }

        public async Task<About.StorageQuotaData> GetGoogleDriveInfoAsync()
        {
            var aboutRequest = _driveService.About.Get();
            aboutRequest.Fields = "storageQuota(limit,usage,usageInDrive,usageInDriveTrash)";
            var aboutResponse = await aboutRequest.ExecuteAsync();
            return aboutResponse.StorageQuota;
        }

        public async Task<IEnumerable<File>> GetFilesAsync()
        {
            var files = new List<File>();
            FileList filesResponse;
            
            do 
            {
                var filesRequest = _driveService.Files.List();
                filesRequest.Fields = "nextPageToken, files(kind,id,name,mimeType,description,trashed,version,originalFilename,fullFileExtension,fileExtension,md5Checksum,size,iconLink)";
                filesRequest.Fields = "*";
                filesRequest.Q = "trashed = false and 'root' in parents";
                filesRequest.Spaces = "drive";
                filesResponse = await filesRequest.ExecuteAsync();
                files.AddRange(filesResponse.Files);
                filesRequest.PageToken = filesResponse.NextPageToken;
            } while (filesResponse.NextPageToken != null);

            return files;
        }

        public async Task<IEnumerable<File>> GetFilesByIdAsync(string id)
        {
            var files = new List<File>();
            FileList filesResponse;

            do 
            {
                var filesRequest = _driveService.Files.List();
                filesRequest.Fields = "nextPageToken, files(kind,id,name,mimeType,description,trashed,version,originalFilename,fullFileExtension,fileExtension,md5Checksum,size,iconLink)";
                filesRequest.Fields = "*";
                filesRequest.Q = $"trashed = false and '{id}' in parents";
                filesRequest.Spaces = "drive";
                filesResponse = await filesRequest.ExecuteAsync();
                files.AddRange(filesResponse.Files);
                filesRequest.PageToken = filesResponse.NextPageToken;
            } while (filesResponse.NextPageToken != null);

            return files;
        }
    }
}
