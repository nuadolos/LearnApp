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
    public class NoteController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Хранит список ролей
        /// </summary>
        private SelectList SourceLoreList { get; }

        private IEnumerable<SourceLore>? LoreList { get; }

        /// <summary>
        /// Получает URL Api для отправки и получения запросов.
        /// Также заполняет SourceLoreList данными из БД.
        /// </summary>
        /// <param name="configuration"></param>
        public NoteController(IConfiguration configuration)
        {
            _baseUrl = configuration.GetSection("NoteAddress").Value;

            LoreList = HttpRequestClient.GetRequestAsync<List<SourceLore>>(_baseUrl, "sources").Result;

            SourceLoreList = new SelectList(
                LoreList, "Id", "Name", new Note().SourceLoreId);
        }

        /// <summary>
        /// Получает запись о материале.
        /// Метод используется с целью сокращение кода.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Note?> GetNoteRecord(string email, int id, string action) => 
            await HttpRequestClient.GetRequestAsync<Note>(_baseUrl, email, id.ToString(), action);

        #region Index/Details

        public async Task<IActionResult> Index()
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
            {
                return BadRequest();
            }

            var note = await HttpRequestClient.GetRequestAsync<List<Note>>(_baseUrl, "User", userName);

            if (note != null)
            {
                foreach (var learn in note)
                {
                    learn.LoreName = GetLoreName(learn.SourceLoreId);
                }

                return View(note);
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

            var note = await GetNoteRecord(userName, id.Value, nameof(Details));

            if (note == null)
                return NotFound(HttpRequestClient.Error);

            note.LoreName = GetLoreName(note.SourceLoreId);

            return View(note);
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
        public async Task<IActionResult> Create(Note note)
        {
            string? userName = User?.Identity?.Name;

            if (ModelState.IsValid && userName != null)
            {
                bool result = await HttpRequestClient.PostRequestAsync(note, _baseUrl, userName);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
                }
            }

            ViewData["SourceLoreId"] = SourceLoreList;
            return View(note);
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

            var note = await GetNoteRecord(userName, id.Value, nameof(Edit));

            ViewData["SourceLoreId"] = SourceLoreList;

            return note != null ? View(note) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PutRequestAsync(note, _baseUrl, id.ToString());

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
                }
            }

            ViewData["SourceLoreId"] = SourceLoreList;
            return View(note);
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

            var note = await GetNoteRecord(userName, id.Value, nameof(Details));

            if (note == null)
                return NotFound(HttpRequestClient.Error);

            note.LoreName = GetLoreName(note.SourceLoreId);

            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Note note)
        {
            //Сериализация массива байтов в строку для вставки в маршрут
            var timeStampString = JsonConvert.SerializeObject(note.Timestamp);

            return await HttpRequestClient.DeleteRequestAsync<Learn>(_baseUrl, note.Id.ToString(), timeStampString) 
                ? RedirectToAction(nameof(Index)) 
                : BadRequest(HttpRequestClient.Error);
        }

        #endregion

        [NonAction]
        private string GetLoreName(int? id)
        {
            foreach (var item in LoreList)
            {
                if (id == item.Id)
                    return item.Name;
            }

            return string.Empty;
        }
    }
}
