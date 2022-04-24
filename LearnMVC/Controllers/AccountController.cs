using LearnHTTP;
using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Запрос на регистрацию новой учетной записи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegister model)
        {
            bool result = await HttpRequestClient.PostRequestAsync(model, "http://localhost:5243/api/ApiAccount/Regist");

            if (result)
            {
                User user = await _userManager.FindByNameAsync(model.Email);
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (HttpRequestClient.Errors != null)
                {
                    foreach (var error in HttpRequestClient.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error?.Message ?? "Неизвестная ошибка");
                    }
                }

                return View(model);
            }    
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null) => View(new UserLogin { ReturnUrl = returnUrl });

        /// <summary>
        /// Запрос на вход в учетную запись
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin model)
        {
            bool result = await HttpRequestClient.PostRequestAsync(model, "http://localhost:5243/api/ApiAccount/Login");

            if (result)
            {
                User user = await _userManager.FindByNameAsync(model.Email);
                await _signInManager.SignInAsync(user, model.RememberMe);
                return !string.IsNullOrEmpty(model.ReturnUrl)
                    ? Redirect(model.ReturnUrl)
                    : RedirectToAction("Index", "Home");
            }
            else
            {
                if (HttpRequestClient.Errors != null)
                {
                    foreach (var error in HttpRequestClient.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error?.Message ?? "Неизвестная ошибка");
                    }
                }

                return View(model);
            }
        }

        /// <summary>
        /// Запрос на ыыход из учетной записи
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #region Реализовать изменение пароля

        /*
         * Создать класс, отвечающий за рассылку сообщений с главной почты.
         * 
         * Применение:
         *      - отправка специального кода на эл. почту пользователя
         *          с целью подтверждения личности при смене пароля
         *          
         * Использование сторонней библиотеки:
         *      - MailKit
         * 
         * Реализация:
         *      - https://metanit.com/sharp/aspnet5/21.1.php
        */

        [HttpGet]
        public IActionResult ChangePassword() => View();

        /// <summary>
        /// Запрос на вход в учетную запись
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserLogin model)
        {
            bool result = await HttpRequestClient.PostRequestAsync(model, "http://localhost:5243/api/ApiAccount/Login");

            if (result)
            {
                User user = await _userManager.FindByNameAsync(model.Email);
                await _signInManager.SignInAsync(user, model.RememberMe);
                return !string.IsNullOrEmpty(model.ReturnUrl)
                    ? Redirect(model.ReturnUrl)
                    : RedirectToAction("Index", "Home");
            }
            else
            {
                if (HttpRequestClient.Errors != null)
                {
                    foreach (var error in HttpRequestClient.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error?.Message ?? "Неизвестная ошибка");
                    }
                }

                return View(model);
            }
        }

        #endregion
    }
}
