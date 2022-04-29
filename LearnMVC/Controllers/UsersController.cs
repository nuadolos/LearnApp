using LearnEF.Entities.IdentityModel;
using LearnHTTP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public class UsersController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Получает URL Api для отправки и получения запросов,
        /// а также управление над всеми учетными записями пользователей
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userManager"></param>
        public UsersController(IConfiguration configuration, UserManager<User> userManager)
        {
            _baseUrl = configuration.GetSection("UsersAddress").Value;
            _userManager = userManager;
        }


        /// <summary>
        /// Получает запись о пользователе.
        /// Метод используется с целью сокращение кода.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<User?> GetUserRecord(string id) =>
            await HttpRequestClient.GetRequestAsync<User>(_baseUrl, id);

        #region Index/Details

        public async Task<IActionResult> Index()
        {
            var users = await HttpRequestClient.GetRequestAsync<List<User>>(_baseUrl);

            return users != null ? View(users) : NotFound(HttpRequestClient.Errors);
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = await GetUserRecord(id);
            return user != null ? View(user) : NotFound(HttpRequestClient.Errors);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserData user)
        {
            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PostRequestAsync(user, _baseUrl);

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

            return View(user);
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var currentUser = await GetUserRecord(id);

            if (currentUser == null)
            {
                return NotFound(HttpRequestClient.Errors);
            }
            else
            {
                UserData user = new UserData
                {
                    Email = currentUser.Email,
                    Id = currentUser.Id,
                    Name = currentUser.Name,
                    Surname = currentUser.Surname,
                    Password = "123123"
                };

                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserData user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PutRequestAsync(user, _baseUrl, id);

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

            return View(user);
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = await GetUserRecord(id);

            return user != null ? View(user) : NotFound(HttpRequestClient.Errors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(User user)
        {
            return await HttpRequestClient.DeleteRequestAsync<User>(_baseUrl, user.Id) 
                ? RedirectToAction(nameof(Index)) 
                : BadRequest(HttpRequestClient.Errors);
        }

        #endregion
    }
}
