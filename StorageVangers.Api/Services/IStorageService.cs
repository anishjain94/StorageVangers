using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;

namespace StorageVangers.Api.Services
{
    public interface IStorageService
    {
        public Task<About.StorageQuotaData> GetGoogleDriveInfoAsync();
        public Task<IEnumerable<File>> GetFilesAsync();
        public Task<IEnumerable<File>> GetFilesByIdAsync(string id);
    }
}
