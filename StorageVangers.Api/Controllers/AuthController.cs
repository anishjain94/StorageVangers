using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace StorageVangers.Api.Controllers
{
    public class AuthController : ControllerBase
    {
        [Route("loginexternal/{id}")]
        public async Task LogInExternal(string id, string redirectUri = "/")
        {
            await HttpContext.ChallengeAsync(id, new AuthenticationProperties { RedirectUri = redirectUri });
        }
    }
}
