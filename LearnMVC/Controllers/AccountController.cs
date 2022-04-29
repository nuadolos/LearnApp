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

        #region Регистрация

        /// <summary>
        /// Запрос на отображение представления
        /// </summary>
        /// <returns></returns>
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
            if (ModelState.IsValid)
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
                }
            }

            return View(model);
        }

        #endregion

        #region Авторизация и разлогирование

        /// <summary>
        /// Запрос на отображение представления
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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
            if (ModelState.IsValid)
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
                }
            }

            return View(model);
        }

        /// <summary>
        /// Запрос на выход из учетной записи
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Восстановление пароля

        /// <summary>
        /// Запрос на отображение представления
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ChangePassword() => View();

        /// <summary>
        /// Запрос на получении секретного кода
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(OnlyEmail model)
        {
            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PostRequestAsync(model, "http://localhost:5243/api/ApiAccount/SendSecretCode");

                if (result)
                    return View("CodeChangePassword", new UserChangePassword { Email = model.Email, CountAttempts = 3 });
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

            return View(model);
        }
        
        /// <summary>
        /// Запрос на восстановление пароля пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CodeChangePassword(UserChangePassword model)
        {
            if (ModelState.IsValid)
            {
                bool result = await HttpRequestClient.PutRequestAsync(model, "http://localhost:5243/api/ApiAccount/ChangePassword");

                if (result)
                    return RedirectToAction("Login", "Account");
                else
                {
                    // Уменьшает лимит попыткок после вызова запроса
                    if (model.CountAttempts > 0)
                    {
                        model.CountAttempts--;
                    }

                    if (HttpRequestClient.Errors != null)
                    {
                        foreach (var error in HttpRequestClient.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error?.Message ?? "Неизвестная ошибка");
                        }
                    }
                }
            }

            return View(model);
        }

        #endregion
    }
}
