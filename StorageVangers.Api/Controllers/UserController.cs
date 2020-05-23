using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageVangers.Api.Models;
using System.Threading.Tasks;

namespace StorageVangers.Api.Controllers
{
    [Authorize]
    public class UserController : ControllerBase
    {
        [Route("getuserinfo")]
        public IActionResult GetUserInfoAsync()
        {
            return new JsonResult(new UserInfo
            {
                UserName = HttpContext.User.Identity.Name
            });
        }

        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
