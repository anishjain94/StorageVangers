using Microsoft.AspNetCore.Mvc;
using StorageVangers.Api.Services;
using System.Threading.Tasks;

namespace StorageVangers.Api.Controllers
{
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
            => (_storageService) = (storageService);

        public async Task<IActionResult> GetDriveInfo()
        {
            var driveInfo = await _storageService.GetGoogleDriveInfoAsync();
            return new JsonResult(driveInfo);
        }
    }
}
