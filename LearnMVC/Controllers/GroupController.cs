using LearnEF.Entities;
using LearnHTTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LearnMVC.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Хранит список ролей
        /// </summary>
        private SelectList GroupTypeList { get; }

        public GroupController(IConfiguration configuration)
        {
            _baseUrl = configuration.GetSection("LearnAddress").Value;

            GroupTypeList = new SelectList(
                new[] { "Равноправный", "Класс"}, "Id", "Name", new Group().GroupTypeId);
        }

        private async Task<Group?> GetGroupRecord(string email, int id, string action) =>
            await HttpRequestClient.GetRequestAsync<Group>(_baseUrl, email, id.ToString(), action);

        #region Index/Details

        public async Task<IActionResult> Index()
        {
            var groups = await HttpRequestClient.GetRequestAsync<List<Group>>(_baseUrl);

            if (groups != null)
            {
                foreach (var group in groups)
                {
                    foreach (var source in GroupTypeList.ToArray())
                    {
                        if (group.GroupTypeId.ToString() == source.Value)
                        {
                            group.TypeName = source.Text;
                            break;
                        }
                    }
                }

                return View(groups);
            }

            return BadRequest(HttpRequestClient.Error);
        }

        public async Task<IActionResult> Details(int? id)
        {
            string? userName = User?.Identity?.Name;

            if (id == null || userName == null)
            {
                return BadRequest();
            }

            var learn = await GetGroupRecord(userName, id.Value, nameof(Details));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Error);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            ViewData["SourceLoreId"] = GroupTypeList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Learn learn)
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

            ViewData["SourceLoreId"] = GroupTypeList;
            return View(learn);
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(int? id)
        {
            string? userName = User?.Identity?.Name;

            if (id == null || userName == null)
            {
                return BadRequest();
            }

            var learn = await GetGroupRecord(userName, id.Value, nameof(Edit));

            ViewData["SourceLoreId"] = GroupTypeList;

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

            ViewData["SourceLoreId"] = GroupTypeList;
            return View(learn);
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id)
        {
            string? userName = User?.Identity?.Name;

            if (id == null || userName == null)
            {
                return BadRequest();
            }

            var learn = await GetGroupRecord(userName, id.Value, nameof(Delete));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("Id, Timestamp")] Learn learn)
        {
            //Сериализация массива байтов в строку для вставки в маршрут
            var timeStampString = JsonConvert.SerializeObject(learn.Timestamp);

            return await HttpRequestClient.DeleteRequestAsync<Learn>(_baseUrl, learn.Id.ToString(), timeStampString)
                ? RedirectToAction(nameof(Index))
                : BadRequest(HttpRequestClient.Error);
        }

        #endregion

        [HttpPost]
        public IActionResult Invite(int groupId)
        {
            //bool result = await HttpRequestClient.PostRequestAsync()

            return View();
        }
    }
}
