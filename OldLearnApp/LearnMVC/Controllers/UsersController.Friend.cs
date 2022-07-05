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
        public async Task<IActionResult> Following()
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            var myFollowing = await HttpRequestClient.GetRequestAsync<List<User>>(_friendUrl, "Following", userName);

            return myFollowing != null ? View(myFollowing) : BadRequest(HttpRequestClient.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Followers()
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            var myFollowers = await HttpRequestClient.GetRequestAsync<List<User>>(_friendUrl, "Followers", userName);

            return myFollowers != null ? View(myFollowers) : BadRequest(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Follow(string userId)
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            return await HttpRequestClient.PostRequestAsync(new object(), _friendUrl, userName, userId)
                ? RedirectToAction(nameof(Details), new { id = userId })
                : BadRequest(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unfollow(string userId)
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            return await HttpRequestClient.DeleteRequestAsync<object>(_friendUrl, userName, userId)
                ? RedirectToAction(nameof(Details), new { id = userId })
                : BadRequest(HttpRequestClient.Error);
        }
    }
}
