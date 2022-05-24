using LearnEF.Entities;
using LearnHTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public partial class LearnController : Controller
    {
        private readonly string _attachUrl;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttachFile(int learnId, IFormFile attachFile)
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            Attach attach = new Attach {
                LearnId = learnId,
                Rating = 0,
                UserId = "1",
                AttachmentDate = DateTime.Now
            };

            if (attachFile != null)
            {
                attach.FileName = attachFile.FileName;

                using (var binaryReader = new BinaryReader(attachFile.OpenReadStream()))
                {
                    attach.FileContent = binaryReader.ReadBytes((int)attachFile.Length);
                }
            }

            return await HttpRequestClient.PostRequestAsync(attach, _attachUrl, userName)
                ? RedirectToAction(nameof(Details), new { id = learnId })
                : BadRequest(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnpinFile(int id, int learnId) =>
            await HttpRequestClient.DeleteRequestAsync<object>(_attachUrl, id.ToString())
                ? RedirectToAction(nameof(Details), new { id = learnId })
                : BadRequest(HttpRequestClient.Error);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRating(int id, int learnId, int rating)
        {
            if (rating < 2 || rating > 5)
            {
                return BadRequest("Оценка должна содержать целое число от 2 до 5");
            }    

            bool result = await HttpRequestClient.PutRequestAsync(
                new object(), _attachUrl, id.ToString(), rating.ToString());

            return result ? RedirectToAction(nameof(Details), new { id = learnId }) : BadRequest(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadStudentFile(int id)
        {
            var attach = await HttpRequestClient.GetRequestAsync<Attach>(_attachUrl, "Get", id.ToString());

            if (attach == null)
                return BadRequest(HttpRequestClient.Error);

            return File(attach.FileContent, ContentTypes(attach.FileName), attach.FileName);
        }
    }
}
