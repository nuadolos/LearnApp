using LearnEF.Entities;
using LearnHTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public partial class LearnController : Controller
    {
        private readonly string _documentUrl;

        [HttpGet]
        public async Task<IActionResult> Documents(int? id)
        {
            if (id == null)
                return BadRequest();

            var documents = await HttpRequestClient.GetRequestAsync<List<LearnDocuments>>(
                _documentUrl, "Learn", id.Value.ToString());

            if (documents == null)
                return BadRequest(HttpRequestClient.Error);

            documents.Insert(0, new LearnDocuments {
                LearnId = id.Value
            });

            return View(documents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadFile(int id, IFormFile uploadFile)
        {
            if (uploadFile == null)
                return RedirectToAction(nameof(Documents), new { id = id });

            LearnDocuments document = new LearnDocuments
            {
                Name = uploadFile.FileName,
                LearnId = id
            };

            using (var binaryReader = new BinaryReader(uploadFile.OpenReadStream()))
            {
                document.FileContent = binaryReader.ReadBytes((int)uploadFile.Length);
            }

            return await HttpRequestClient.PostRequestAsync(document, _documentUrl)
                ? RedirectToAction(nameof(Documents), new { id = id })
                : BadRequest(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Download(int docId)
        {
            var document = await HttpRequestClient.GetRequestAsync<LearnDocuments>(
                _documentUrl, docId.ToString());

            if (document == null)
                return BadRequest(HttpRequestClient.Error);

            return File(document.FileContent, ContentTypes(document.Name), document.Name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpin(int learnId, int docId) =>
            await HttpRequestClient.DeleteRequestAsync<object>(_documentUrl, docId.ToString())
                ? RedirectToAction(nameof(Documents), new { id = learnId })
                : BadRequest(HttpRequestClient.Error);

        [NonAction]
        private string ContentTypes(string fileName)
        {
            string[] types = fileName.Split('.');
            string type = types[types.Length - 1].Insert(0, ".");

            return type switch
            {
                ".txt" => "text/plain",
                ".css" => "text/css",
                ".html" => "text/html",
                ".rtf" => "text/rtf",
                ".xml" => "text / xml",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".svg" => "image/svg+xml",
                ".webp" => "image/webp",
                ".gif" => "image/gif",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".pdf" => "application/pdf",
                ".mp4" => "video/mp4",
                ".mpeg" => "video/mpeg",
                _ => "multipart/mixed"
            };
        }
    }
}
