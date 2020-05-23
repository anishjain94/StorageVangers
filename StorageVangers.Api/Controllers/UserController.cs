using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StorageVangers.Api.Data;
using StorageVangers.Api.Models;
using System.Threading.Tasks;

namespace StorageVangers.Api.Controllers
{
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(SignInManager<AppUser> signInManager) => _signInManager = signInManager;

        [Route("getuserinfo")]
        public IActionResult GetUserInfoAsync()
        {
            return new JsonResult(new UserInfo
            {
                UserName = HttpContext.User.Identity.Name
            });
        }

        [Route("logout")]
        public async Task LogOut()
        {
            await HttpContext.SignOutAsync("Identity.External", new AuthenticationProperties { RedirectUri = "/" });
        }
    }
}
