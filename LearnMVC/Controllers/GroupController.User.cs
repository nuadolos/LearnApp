using LearnEF.Entities;
using LearnHTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public partial class GroupController : Controller
    {
        private readonly string _userUrl;

        [HttpGet]
        public async Task<IActionResult> Invite()
        {
            var id = RouteData?.Values["id"]?.ToString();

            string? userName = User?.Identity?.Name;

            if (userName == null || id == null)
                return BadRequest();

            bool result = await HttpRequestClient.PostRequestAsync(new object(), _userUrl, userName, id);

            return result ? RedirectToAction("UserIndex") : BadRequest(HttpRequestClient.Error.Message);
        }
    }
}
