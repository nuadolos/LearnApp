using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using LearnEF.Entities.ErrorModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Запрос на регистрацию новой учетной записи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Regist")]
        public async Task<IActionResult> Regist([FromBody] UserRegister model)
        {
            List<ValidateError>? errors = null;

            //Проверяет на совпадение паролей
            if (model.Password != model.PasswordConfirm)
            {
                errors = new List<ValidateError>();
                errors.Add(new ValidateError { Message = "Пароли не совпадают" });
                return BadRequest(errors);
            }

            //Проверяет на валидность данных
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };

                //Создает новую запись о пользователе в БД
                var result = await _userManager.CreateAsync(user, model.Password);

                //В случае безошибочного создания записи, заходит в учетную запись
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return Ok();
                }
                else
                {
                    //Получение ошибок при создании записи
                    errors = new List<ValidateError>();

                    foreach (var error in result.Errors)
                    {
                        errors.Add(new ValidateError { Message = error.Description });
                    }
                }
            }

            //Возвращает ошибку 400, связанную с неправильным вводом данных
            return BadRequest(errors);
        }

        /// <summary>
        /// Запрос на вход в учетную запись
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            List<ValidateError>? errors = null;

            //Проверяет на валидность данных
            if (ModelState.IsValid)
            {
                //Входит в учетную записи
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                //В случае ввода верных данных, возвращает код 200
                if (result.Succeeded)
                    //return !string.IsNullOrEmpty(model.ReturnUrl) ? Redirect(model.ReturnUrl) : RedirectToAction("", "");
                    return Ok();
                else
                {
                    errors = new List<ValidateError>();

                    errors.Add(new ValidateError { Message = "Неправельный логин или пароль" });
                }
            }

            //Возвращает ошибку 400, связанную с неправильным вводом данных
            return BadRequest(errors);
        }

        /// <summary>
        /// Запрос на ыыход из учетной записи
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            //Производит выход из учетной записи
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
