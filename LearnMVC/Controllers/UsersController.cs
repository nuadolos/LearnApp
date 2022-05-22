﻿using LearnEF.Entities.IdentityModel;
using LearnHTTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class UsersController : Controller
    {
        /// <summary>
        /// Базовая ссылка для обращения к LearnAPI
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Получает URL Api для отправки и получения запросов
        /// </summary>
        /// <param name="configuration"></param>
        public UsersController(IConfiguration configuration)
        {
            _baseUrl = configuration.GetSection("UsersAddress").Value;
            _friendUrl = configuration.GetSection("FriendAddress").Value;
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

            return users != null ? View(users) : NotFound(HttpRequestClient.Error);
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = await GetUserRecord(id);
            return user != null ? View(user) : NotFound(HttpRequestClient.Error);
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
                    ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
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
                return NotFound(HttpRequestClient.Error);
            }
            else
            {
                UserData user = new UserData
                {
                    Email = currentUser.Email,
                    Id = currentUser.Id,
                    Name = currentUser.Name,
                    Surname = currentUser.Surname,
                    Enabled = currentUser.LockoutEnabled,
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
                    ModelState.AddModelError(string.Empty, HttpRequestClient.Error.Message);
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

            return user != null ? View(user) : NotFound(HttpRequestClient.Error);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(User user)
        {
            return await HttpRequestClient.DeleteRequestAsync<User>(_baseUrl, user.Id) 
                ? RedirectToAction(nameof(Index)) 
                : BadRequest(HttpRequestClient.Error);
        }

        #endregion
    }
}
