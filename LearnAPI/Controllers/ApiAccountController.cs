using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using LearnEF.Entities.ErrorModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ApiAccountController(UserManager<User> userManager, SignInManager<User> signInManager)
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

            //Проверяет на наличие пустых полей
            if (string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Surname) ||
                string.IsNullOrEmpty(model.Name) ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.PasswordConfirm))
            {
                errors = new List<ValidateError>();
                errors.Add(new ValidateError("Все поля должны быть заполнены"));
                return BadRequest(errors);
            }

            //Проверяет совпадение паролей
            if (model.Password != model.PasswordConfirm)
            {
                errors = new List<ValidateError>();
                errors.Add(new ValidateError("Пароли не совпадают"));
                return BadRequest(errors);
            }

            //Создание нового пользователя
            User user = new User { Email = model.Email, UserName = model.Email, Surname = model.Surname, Name = model.Name };

            //Создает новую запись о пользователе в БД
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Возвращает код 200, означающий что учетная запись успешно создана
                return Ok();
            }
            else
            {
                //Получение ошибок при создании записи
                errors = new List<ValidateError>();

                foreach (var error in result.Errors)
                {
                    errors.Add(new ValidateError(error.Description));
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

            if (string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Password))
            {
                errors = new List<ValidateError>();
                errors.Add(new ValidateError("Все поля должны быть заполнены"));
                return BadRequest(errors);
            }

            //Проверяет на валидность данных
            if (ModelState.IsValid)
            {
                //Входит в учетную записи
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                //В случае ввода верных данных, возвращает код 200
                if (result.Succeeded)
                    return Ok();
                else
                {
                    errors = new List<ValidateError>();

                    errors.Add(new ValidateError("Неправильный логин или пароль"));
                }
            }

            //Возвращает ошибку 400, связанную с неправильным вводом данных
            return BadRequest(errors);
        }

        /// <summary>
        /// Запрос на изменение пароля
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("changed={id}")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePassword model)
        {
            List<ValidateError>? errors = null;

            User user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                #region Изменение пароля без подтверждения

                //var passwordValidator =
                //    HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                //var passwordHasher =
                //    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                //IdentityResult result = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);

                //if (result.Succeeded)
                //{
                //    user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                //    await _userManager.UpdateAsync(user);
                //    return Ok();
                //}

                #endregion

                IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                    return Ok();
                else
                {
                    errors = new List<ValidateError>();

                    foreach (var error in result.Errors)
                    {
                        errors.Add(new ValidateError(error.Description));
                    }
                }
            }
            else
            {
                errors = new List<ValidateError>();
                errors.Add(new ValidateError("Пользователь не найден"));
            }

            return BadRequest(errors);
        }
    }
}
