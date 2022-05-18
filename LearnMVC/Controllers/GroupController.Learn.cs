using LearnEF.Entities;
using LearnHTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public partial class GroupController : Controller
    {
        private readonly string _learnUrl;

        [HttpGet]
        public async Task<IActionResult> Learns(int id)
        {
            var learns = await HttpRequestClient.GetRequestAsync<List<Learn>>(_learnUrl, "Group", id.ToString());

            return learns != null ? View(learns) : BadRequest(HttpRequestClient.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Details22(int id)
        {
            var learns = await HttpRequestClient.GetRequestAsync<List<Learn>>(_learnUrl, "Group", id.ToString());

            return learns != null ? View(learns) : BadRequest(HttpRequestClient.Error);
        }

        public IActionResult CreateLearn() => View();

        [HttpPost]
        public async Task<IActionResult> CreateLearn([FromBody] Learn learn)
        {
            bool result = await HttpRequestClient.PostRequestAsync(learn, _learnUrl);

            return result ? RedirectToAction(nameof(Learns)) : BadRequest(HttpRequestClient.Error);
        }
    }
}
