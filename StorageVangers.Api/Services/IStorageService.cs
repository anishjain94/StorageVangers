using System.Threading.Tasks;
using static Google.Apis.Drive.v3.Data.About;

namespace StorageVangers.Api.Services
{
    public interface IStorageService
    {
        public Task<StorageQuotaData> GetGoogleDriveInfoAsync();
    }
}
