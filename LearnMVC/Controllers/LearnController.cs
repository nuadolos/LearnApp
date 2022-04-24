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
        private async Task<Learn?> GetLearnRecord(int id) => 
            await HttpRequestClient.GetRequestAsync<Learn>(_baseUrl, id.ToString());

        #region Index/Details

        public async Task<IActionResult> Index()
        {
            var learns = await HttpRequestClient.GetRequestAsync<List<Learn>>(_baseUrl);

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
            if (id == null)
            {
                return BadRequest();
            }

            var learn = await GetLearnRecord(id.Value);
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
            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PostRequestAsync(learn, _baseUrl);

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
            if (id == null)
            {
                return BadRequest();
            }

            var learn = await GetLearnRecord(id.Value);

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
            if (id == null)
            {
                return BadRequest();
            }

            var learn = await GetLearnRecord(id.Value);

            return learn != null ? View(learn) : NotFound(HttpRequestClient.Errors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("Id, Timestamp")] Learn learn)
        {
            //Сериализация массива байтов в строку для вставки в маршрут
            var timeStampString = JsonConvert.SerializeObject(learn.Timestamp);

            await HttpRequestClient.DeleteRequestAsync<Learn>(_baseUrl, learn.Id.ToString(), timeStampString);

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
