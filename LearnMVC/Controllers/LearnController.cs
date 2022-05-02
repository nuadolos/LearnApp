using LearnHTTP;
using LearnEF.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using LearnEF.Entities.ErrorModel;

namespace LearnMVC.Controllers
{
    [Authorize]
    public class LearnController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Хранит список ролей
        /// </summary>
        private SelectList SourceLoreList { get; }

        /// <summary>
        /// Получает URL Api для отправки и получения запросов.
        /// Также заполняет SourceLoreList данными из БД.
        /// </summary>
        /// <param name="repo"></param>
        public LearnController(IConfiguration configuration)
        {
            _baseUrl = configuration.GetSection("LearnAddress").Value;

            SourceLoreList = new SelectList(
                HttpRequestClient.GetRequestAsync<List<SourceLore>>(_baseUrl, "sources").Result, "Id", "Name", new Learn().SourceLoreId);
        }

        /// <summary>
        /// Получает запись о материале.
        /// Метод используется с целью сокращение кода.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Learn?> GetLearnRecord(string email, int id, string action) => 
            await HttpRequestClient.GetRequestAsync<Learn>(_baseUrl, email, id.ToString(), action);

        #region Index/Details

        public async Task<IActionResult> Index()
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
            {
                return BadRequest();
            }

            var learns = await HttpRequestClient.GetRequestAsync<List<Learn>>(_baseUrl, userName);

            if (learns != null)
            {
                foreach (var learn in learns)
                {
                    foreach (var source in SourceLoreList.ToArray())
                    {
                        if (learn.SourceLoreId.ToString() == source.Value)
                        {
                            learn.LoreName = source.Text;
                            break;
                        }
                    }
                }

                return View(learns);
            }

            return BadRequest(HttpRequestClient.Errors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            string? userName = User?.Identity?.Name;

            if (id == null || userName == null)
            {
                return BadRequest();
            }

            var learn = await GetLearnRecord(userName, id.Value, nameof(Details));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Errors);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            ViewData["SourceLoreId"] = SourceLoreList;
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
                    if (HttpRequestClient.Errors != null)
                    {
                        foreach (var error in HttpRequestClient.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error?.Message ?? "Неизвестная ошибка");
                        }
                    }
                }
            }

            ViewData["SourceLoreId"] = SourceLoreList;
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

            var learn = await GetLearnRecord(userName, id.Value, nameof(Edit));

            ViewData["SourceLoreId"] = SourceLoreList;

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Errors);
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
                    if (HttpRequestClient.Errors != null)
                    {
                        foreach (var error in HttpRequestClient.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error?.Message ?? "Неизвестная ошибка");
                        }
                    }
                }
            }

            ViewData["SourceLoreId"] = SourceLoreList;
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

            var learn = await GetLearnRecord(userName, id.Value, nameof(Delete));

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Errors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("Id, Timestamp")] Learn learn)
        {
            //Сериализация массива байтов в строку для вставки в маршрут
            var timeStampString = JsonConvert.SerializeObject(learn.Timestamp);

            return await HttpRequestClient.DeleteRequestAsync<Learn>(_baseUrl, learn.Id.ToString(), timeStampString) 
                ? RedirectToAction(nameof(Index)) 
                : BadRequest(HttpRequestClient.Errors);
        }

        #endregion
    }
}
