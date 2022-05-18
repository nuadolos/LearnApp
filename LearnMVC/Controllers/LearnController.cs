using LearnEF.Entities;
using LearnHTTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnMVC.Controllers
{
    [Authorize]
    public class LearnController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        public LearnController(IConfiguration configuration) =>
            _baseUrl = configuration.GetSection("LearnAddres").Value;

        private async Task<Learn?> GetLearnRecord(int groupId, string email, int id, string action) =>
            await HttpRequestClient.GetRequestAsync<Learn>(_baseUrl, groupId.ToString(), email, id.ToString(), action);

        #region Index/Details

        public async Task<IActionResult> Index(int id)
        {
            var learns = await HttpRequestClient.GetRequestAsync<List<Learn>>(_baseUrl, "Group", id.ToString());

            return learns != null ? View(learns) : BadRequest(HttpRequestClient.Error);
        }

        public async Task<IActionResult> Details(int? id, int groupId)
        {
            string? userName = User?.Identity?.Name;

            if (id == null || userName == null)
            {
                return BadRequest();
            }

            var learn = await GetLearnRecord(groupId, userName, id.Value, nameof(Details));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Error);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int groupId, Learn learn)
        {
            string? userName = User?.Identity?.Name;

            if (ModelState.IsValid && userName != null)
            {
                bool result = await HttpRequestClient.PostRequestAsync(learn, _baseUrl, userName);

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

        #region Edit

        public async Task<IActionResult> Edit(int? id, int groupId)
        {
            string? userName = User?.Identity?.Name;

            if (id == null || userName == null)
            {
                return BadRequest();
            }

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

            if (id == null || userName == null)
            {
                return BadRequest();
            }

            var learn = await GetLearnRecord(groupId, userName, id.Value, nameof(Delete));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Learn learn)
        {
            //Сериализация массива байтов в строку для вставки в маршрут
            var timeStampString = JsonConvert.SerializeObject(learn.Timestamp);

            return await HttpRequestClient.DeleteRequestAsync<Group>(_baseUrl, learn.Id.ToString(), timeStampString)
                ? RedirectToAction(nameof(Index))
                : BadRequest(HttpRequestClient.Error);
        }

        #endregion
    }
}
