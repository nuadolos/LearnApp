using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using LearnEF.Entities.ErrorModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LearnHTTP.EmailService;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public ApiAccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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

            // Проверяет на наличие пустых полей
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

            // Проверяет совпадение паролей
            if (model.Password != model.PasswordConfirm)
            {
                errors = new List<ValidateError>();
                errors.Add(new ValidateError("Пароли не совпадают"));
                return BadRequest(errors);
            }

            // Создание нового пользователя
            User user = new User { Email = model.Email, UserName = model.Email, Surname = model.Surname, Name = model.Name };

            // Создает новую запись о пользователе в БД
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Присваение роли новому пользователю
                await _userManager.AddToRoleAsync(user, "student");

                // Возвращает код 200, означающий что учетная запись успешно создана
                return Ok();
            }
            else
            {
                // Получение ошибок при создании записи
                errors = new List<ValidateError>();

                foreach (var error in result.Errors)
                {
                    errors.Add(new ValidateError(error.Description));
                }
            }

            // Возвращает ошибку 400, связанную с неправильным вводом данных
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

            // Входит в учетную записи
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            // В случае ввода верных данных, возвращает код 200
            if (result.Succeeded)
            {
                if ((await _userManager.FindByEmailAsync(model.Email)).LockoutEnabled)
                    return Ok();
                else
                {
                    errors = new List<ValidateError>();

                    errors.Add(new ValidateError("Аккаунт заблокирован"));
                }
            }
            else
            {
                errors = new List<ValidateError>();

                errors.Add(new ValidateError("Неправильный логин или пароль"));
            }

            //Возвращает ошибку 400, связанную с неправильным вводом данных
            return BadRequest(errors);
        }

        /// <summary>
        /// Запрос на получение секретного кода
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SendSecretCode")]
        public async Task<IActionResult> SendSecretCode([FromBody] OnlyEmail model)
        {
            List<ValidateError>? errors = null;

            // Поиск пользователя по почте
            User user = await _userManager.FindByNameAsync(model.Email);

            if (user != null)
            {
                // Проверяет наличие блокировки и ее просроченность
                if (user.CodeTimeBlock == null || user.CodeTimeBlock < DateTime.UtcNow)
                {
                    // Создает секретный код и сохраняет в базе данных
                    Random rnd = new Random();
                    user.Code = rnd.Next(100_000, 1_000_000).ToString();

                    // Обновляет данные пользователя
                    await _userManager.UpdateAsync(user);

                    // Создает сообщение с секретным кодом и отравляет пользователю на почту
                    var message = new Message(new string[] { model.Email }, "Код подтвеждения личности",
                        $"Ваш код подтверждения для восстановления пароля: {user.Code}\nЕсли код был выслан не вами, то проигнорируйте сообщение.", null);
                    await _emailSender.SendEmailAsync(message);

                    return Ok();
                }
                else
                {
                    errors = new List<ValidateError>();
                    errors.Add(new ValidateError("Исчерпан лимит попыток ввода секретного кода"));
                }
            }
            else
            {
                errors = new List<ValidateError>();
                errors.Add(new ValidateError($"Пользователя с почтой {model.Email} не существует"));
            }

            return BadRequest(errors);
        }

        /// <summary>
        /// Запрос на восстановление пароля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePassword model)
        {
            List<ValidateError>? errors = null;

            // Поиск пользователя по почте
            User user = await _userManager.FindByNameAsync(model.Email);

            if (user != null)
            {
                // Проверяет кол-во попыток смены пароля
                if (model.CountAttempts > 0)
                {
                    // Проверяет совпадение двух секретных кодов 
                    if (model.CodeInMessage == user.Code)
                    {
                        var passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                        var passwordHasher =
                            HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                        // Проверяет корректность пароля
                        IdentityResult result = await passwordValidator.ValidateAsync(_userManager, user, model.Password);

                        if (result.Succeeded)
                        {
                            // Удаляет секретный код и блокировку смены пароля
                            user.Code = null;
                            user.CodeTimeBlock = null;

                            // Использование хэш-функции для пароля
                            user.PasswordHash = passwordHasher?.HashPassword(user, model.Password);

                            // Обновляет данные пользователя
                            await _userManager.UpdateAsync(user);

                            return Ok();
                        }
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
                        errors.Add(new ValidateError($"Код подтверждения указан неверно. Кол-во попыток: {--model.CountAttempts}"));
                    }
                }
                else
                {
                    if (user.CodeTimeBlock == null)
                    {
                        // Блокировка смены пароля на час
                        user.CodeTimeBlock = DateTime.UtcNow.AddHours(1);

                        // Обновляет данные пользователя
                        await _userManager.UpdateAsync(user);
                    }

                    errors = new List<ValidateError>();
                    errors.Add(new ValidateError($"Исчерпан лимит попыток. Повторите попытку через час"));
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
