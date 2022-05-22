using LearnEF.Entities.IdentityModel;
using LearnHTTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public partial class UsersController : Controller
    {
        private readonly string _friendUrl;

        [HttpGet]
        public async Task<IActionResult> Friends()
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            var friends = await HttpRequestClient.GetRequestAsync<List<User>>(_friendUrl, "Friends", userName);

            return friends != null ? View(friends) : BadRequest(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeFriends()
        {
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndFriendship()
        {

        }
    }
}
