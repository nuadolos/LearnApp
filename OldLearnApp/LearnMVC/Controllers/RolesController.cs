using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using LearnHTTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Получает URL Api для отправки и получения запросов
        /// </summary>
        /// <param name="configuration"></param>
        public RolesController(IConfiguration configuration) =>
            _baseUrl = configuration.GetSection("RolesAddress").Value;

        #region Index

        public async Task<IActionResult> Index()
        {
            var roles = await HttpRequestClient.GetRequestAsync<List<IdentityRole>>(_baseUrl);

            return roles != null ? View(roles) : NotFound(HttpRequestClient.Error);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role role)
        {
            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PostRequestAsync(role, _baseUrl);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
                }
            }

            return View(role);
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(string id) 
        {
            var userRoles = await HttpRequestClient.GetRequestAsync<UserRoles>(_baseUrl, "User", id);

            return userRoles != null ? View(userRoles) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IList<string> roles) =>
            await HttpRequestClient.PutRequestAsync(roles, _baseUrl, id)
            ? RedirectToAction("Index", "Users")
            : BadRequest(HttpRequestClient.Error);

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var role = await HttpRequestClient.GetRequestAsync<IdentityRole>(_baseUrl, id);

            return role != null ? View(role) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IdentityRole role) =>
            await HttpRequestClient.DeleteRequestAsync<IdentityRole>(_baseUrl, role.Id)
                ? RedirectToAction(nameof(Index))
                : BadRequest(HttpRequestClient.Error);

        #endregion
    }
}
