using LearnEF.Entities;
using LearnHTTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LearnMVC.Controllers
{
    [Authorize]
    public partial class GroupController : Controller
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
            _baseUrl = configuration.GetSection("GroupAddress").Value;
            _userUrl = configuration.GetSection("GroupUserAddress").Value;
            _learnUrl = configuration.GetSection("LearnAddres").Value;

            List<GroupType> groupTypes = new List<GroupType>
            {
                new GroupType { Id = 1, Name = "Равноправный"},
                new GroupType { Id = 2, Name = "Класс"}
            };

            GroupTypeList = new SelectList(
                groupTypes, "Id", "Name", new Group().GroupTypeId);
        }

        private async Task<Group?> GetGroupRecord(string email, int id, string action) =>
            await HttpRequestClient.GetRequestAsync<Group>(_baseUrl, email, id.ToString(), action);

        #region Index/Details

        public async Task<IActionResult> Index()
        {
            var groups = await HttpRequestClient.GetRequestAsync<List<Group>>(_baseUrl);

            return groups != null ? View(groups) : BadRequest(HttpRequestClient.Error);
        }

        public async Task<IActionResult> MyIndex()
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            var userGroups = await HttpRequestClient.GetRequestAsync<List<Group>>(_baseUrl, "MyGroup", userName);

            return userGroups != null ? View(userGroups) : BadRequest(HttpRequestClient.Error);
        }

        public async Task<IActionResult> MemberIndex()
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return BadRequest();

            var userGroups = await HttpRequestClient.GetRequestAsync<List<Group>>(_baseUrl, "Member", userName);

            return userGroups != null ? View(userGroups) : BadRequest(HttpRequestClient.Error);
        }

        public async Task<IActionResult> Details(int? id)
        {
            string? userName = User?.Identity?.Name;

            if (id == null || userName == null)
            {
                return BadRequest();
            }

            var group = await GetGroupRecord(userName, id.Value, nameof(Details));

            return group != null ? View(group) : NotFound(HttpRequestClient.Error);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            ViewData["GroupTypeId"] = GroupTypeList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group group)
        {
            string? userName = User?.Identity?.Name;

            if (ModelState.IsValid && userName != null)
            {
                bool result = await HttpRequestClient.PostRequestAsync(group, _baseUrl, userName);

                if (result)
                    return RedirectToAction(nameof(MyIndex));
                else
                {
                    ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
                }
            }

            ViewData["GroupTypeId"] = GroupTypeList;
            return View(group);
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

            var group = await GetGroupRecord(userName, id.Value, nameof(Edit));

            ViewData["GroupTypeId"] = GroupTypeList;

            return group != null ? View(group) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Group group)
        {
            if (id != group.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PutRequestAsync(group, _baseUrl, id.ToString());

                if (result)
                    return RedirectToAction(nameof(MyIndex));
                else
                {
                    ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
                }
            }

            ViewData["GroupTypeId"] = GroupTypeList;
            return View(group);
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

            var group = await GetGroupRecord(userName, id.Value, nameof(Delete));

            return group != null ? View(group) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Group group)
        {
            //Сериализация массива байтов в строку для вставки в маршрут
            var timeStampString = JsonConvert.SerializeObject(group.Timestamp);

            return await HttpRequestClient.DeleteRequestAsync<Group>(_baseUrl, group.Id.ToString(), timeStampString)
                ? RedirectToAction(nameof(MyIndex))
                : BadRequest(HttpRequestClient.Error);
        }

        #endregion
    }
}
