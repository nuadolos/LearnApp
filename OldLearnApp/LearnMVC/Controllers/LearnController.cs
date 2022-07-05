using LearnEF.Entities;
using LearnEF.Entities.Base;
using LearnEF.Entities.WebModel;
using LearnHTTP;
using LearnMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnMVC.Controllers
{
    [Authorize]
    [Route("g/{groupId}/l/{action}/{id?}")]
    public partial class LearnController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        public LearnController(IConfiguration configuration)
        {
            _baseUrl = configuration.GetSection("LearnAddress").Value;
            _documentUrl = configuration.GetSection("LearnDocAddress").Value;
            _attachUrl = configuration.GetSection("AttachAddress").Value;
        }

        private async Task<Learn?> GetLearnRecord(int groupId, string email, int id, string action) =>
            await HttpRequestClient.GetRequestAsync<Learn>(_baseUrl, groupId.ToString(), email, id.ToString(), action);

        #region Index/Details

        public async Task<IActionResult> Index(int groupId)
        {
            var learns = await HttpRequestClient.GetRequestAsync<List<Learn>>(_baseUrl, "Group", groupId.ToString());

            return learns != null ? View(learns) : BadRequest(HttpRequestClient.Error);
        }

        public async Task<IActionResult> Details(int? id, int groupId)
        {
            string? userName = User?.Identity?.Name;
            
            if (id == null || userName == null)
                return BadRequest();

            var learn = await GetLearnRecord(groupId, userName, id.Value, nameof(Details));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Error);
        }

        #endregion

        #region Create

        public IActionResult Create(int groupId) =>
            View(new LearnViewModel { GroupId = groupId });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LearnViewModel learnView)
        {
            string? userName = User?.Identity?.Name;

            if (!ModelState.IsValid || userName == null)
                return View(learnView);

            FullLearn fullLearn = new FullLearn
            {
                Title = learnView.Title,
                Description = learnView.Description,
                CreateDate = learnView.CreateDate,
                Deadline = learnView.Deadline,
                GroupId = learnView.GroupId
            };

            if (learnView.FilesContent != null)
            {
                fullLearn.Files = new List<Document>();

                foreach (var item in learnView.FilesContent)
                {
                    Document document = new Document { Name = item.FileName };

                    using (var binaryReader = new BinaryReader(item.OpenReadStream()))
                    {
                        document.FileContent = binaryReader.ReadBytes((int)item.Length);
                    }

                    fullLearn.Files.Add(document);
                }
            }

            if (!await HttpRequestClient.PostRequestAsync(fullLearn, _baseUrl, userName))
            {
                ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
                return View(learnView);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(int? id, int groupId)
        {
            string? userName = User?.Identity?.Name;

            if (userName == null || id == null)
                return BadRequest();

            var learn = await GetLearnRecord(groupId, userName, id.Value, nameof(Edit));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Learn learn)
        {
            if (id != learn.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PutRequestAsync(learn, _baseUrl, id.ToString());

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
                }
            }

            return View(learn);
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id, int groupId)
        {
            string? userName = User?.Identity?.Name;

            if (userName == null || id == null)
                return BadRequest();

            var learn = await GetLearnRecord(groupId, userName, id.Value, nameof(Delete));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Learn learn) =>
            await HttpRequestClient.DeleteRequestAsync<object>(_baseUrl, learn.Id.ToString())
                ? RedirectToAction(nameof(Index))
                : BadRequest(HttpRequestClient.Error);

        #endregion
    }
}
