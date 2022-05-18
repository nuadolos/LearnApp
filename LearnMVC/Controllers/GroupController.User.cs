using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnHTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public partial class GroupController : Controller
    {
        private readonly string _userUrl;

        [HttpGet]
        public async Task<IActionResult> Members(int id)
        {
            var users = await HttpRequestClient.GetRequestAsync<List<User>>(_userUrl, "Group", id.ToString());

            return users != null ? View(users) : BadRequest(HttpRequestClient.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Invite()
        {
            var id = RouteData?.Values["id"]?.ToString();

            string? userName = User?.Identity?.Name;

            if (userName == null || id == null)
                return BadRequest();

            bool result = await HttpRequestClient.PostRequestAsync(new object(), _userUrl, "Invite", userName, id);

            return result ? RedirectToAction(nameof(MyIndex)) : BadRequest(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            bool result = await HttpRequestClient.PostRequestAsync(new object(), _userUrl, "Join", id.ToString(), userName);

            return result ? RedirectToAction(nameof(MemberIndex)) : BadRequest(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kick(int id, string userId)
        {
            bool result = await HttpRequestClient.DeleteRequestAsync<object>(_userUrl, id.ToString(), userId);

            return result ? RedirectToAction(nameof(Members)) : BadRequest(HttpRequestClient.Error);
        }
    }
}
