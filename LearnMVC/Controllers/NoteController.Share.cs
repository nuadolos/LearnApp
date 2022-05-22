using LearnEF.Entities;
using LearnEF.Entities.WebModel;
using LearnHTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public partial class NoteController : Controller
    {
        private readonly string _shareUrl;

        [HttpGet]
        public IActionResult ToOpenAccess(int? id)
        {
            if (id == null)
                return BadRequest();

            return View(new OpenAccessNote { NoteId = id.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToOpenAccess(OpenAccessNote shareNote)
        {
            if (!ModelState.IsValid)
                return View(shareNote);

            if (await HttpRequestClient.PostRequestAsync(shareNote, _shareUrl))
                return RedirectToAction("Details", "Note", new { id = shareNote.NoteId });
            else
            {
                ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
            }

            return View(shareNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PrivateNote(int id, string userId) =>
            await HttpRequestClient.DeleteRequestAsync<object>(_shareUrl, id.ToString(), userId)
                ? RedirectToAction("Details", "Note", new { id = id })
                : BadRequest(HttpRequestClient.Error);
    }
}
