using LearnEF.Entities;
using LearnHTTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class SourceLoreController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Получает URL Api для отправки и получения запросов
        /// </summary>
        /// <param name="repo"></param>
        public SourceLoreController(IConfiguration configuration) => 
            _baseUrl = configuration.GetSection("SourceLoreAddress").Value;


        /// <summary>
        /// Получает запись о ресурсе.
        /// Метод используется с целью сокращение кода.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<SourceLore?> GetSourceRecord(int id) =>
            await HttpRequestClient.GetRequestAsync<SourceLore>(_baseUrl, id.ToString());

        #region Index/Details

        public async Task<IActionResult> Index()
        {
            var sources = await HttpRequestClient.GetRequestAsync<List<SourceLore>>(_baseUrl);

            return sources != null ? View(sources) : NotFound(HttpRequestClient.Errors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var source = await GetSourceRecord(id.Value);
            return source != null ? View(source) : NotFound(HttpRequestClient.Errors);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SourceLore source)
        {
            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PostRequestAsync(source, _baseUrl);

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

            return View(source);
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var source = await GetSourceRecord(id.Value);

            return source != null ? View(source) : NotFound(HttpRequestClient.Errors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SourceLore source)
        {
            if (id != source.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PutRequestAsync(source, _baseUrl, id.ToString());

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

            return View(source);
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var source = await GetSourceRecord(id.Value);

            return source != null ? View(source) : NotFound(HttpRequestClient.Errors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("Id, Timestamp")] SourceLore source)
        {
            //Сериализация массива байтов в строку для вставки в маршрут
            var timeStampString = JsonConvert.SerializeObject(source.Timestamp);

            await HttpRequestClient.DeleteRequestAsync<Learn>(_baseUrl, source.Id.ToString(), timeStampString);

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
