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
    public class ApiAccountController : ControllerBase
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
        public async Task<IActionResult> RegistAsync([FromBody] UserRegister model)
        {
            // Проверяет на наличие пустых полей
            if (string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Surname) ||
                string.IsNullOrEmpty(model.Name) ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.PasswordConfirm))
            {
                return BadRequest(new ValidateError("Все поля должны быть заполнены"));
            }

            // Проверяет совпадение паролей
            if (model.Password != model.PasswordConfirm)
                return BadRequest(new ValidateError("Пароли не совпадают"));

            // Создание нового пользователя
            User user = new User { Email = model.Email, UserName = model.Email, Surname = model.Surname, Name = model.Name };

            // Создает новую запись о пользователе в БД
            var result = await _userManager.CreateAsync(user, model.Password);

            // Проверяет, создалась ли учетная запись
            if (!result.Succeeded)
                return BadRequest(new ValidateError(result.Errors.ToArray()[0].Description));

            // Присваение роли новому пользователю
            await _userManager.AddToRoleAsync(user, "common");

            // Возвращает код 200, означающий что учетная запись успешно создана
            return Ok();
        }

        /// <summary>
        /// Запрос на вход в учетную запись
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLogin model)
        {
            if (string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new ValidateError("Все поля должны быть заполнены"));
            }

            // Входит в учетную записи
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            // Проверяет, вошел ли пользователь в учетную запись
            if (!result.Succeeded)
                return BadRequest(new ValidateError("Неправильный логин или пароль"));

            // Проверяет, заблокирова ли учетная запись
            if (!(await _userManager.FindByEmailAsync(model.Email)).LockoutEnabled)
                return BadRequest(new ValidateError("Аккаунт заблокирован"));

            return Ok();
        }

        /// <summary>
        /// Запрос на получение секретного кода
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SendSecretCode")]
        public async Task<IActionResult> SendSecretCodeAsync([FromBody] OnlyEmail model)
        {
            // Поиск пользователя по почте
            User user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
                return BadRequest(new ValidateError($"Пользователя с почтой {model.Email} не существует"));

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
                return BadRequest(new ValidateError("Исчерпан лимит попыток ввода секретного кода"));
            }
        }

        /// <summary>
        /// Запрос на восстановление пароля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePassword model)
        {
            // Поиск пользователя по почте
            User user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
                return BadRequest(new ValidateError("Пользователь не найден"));

            // Проверяет кол-во попыток смены пароля
            if (model.CountAttempts <= 0)
            {
                if (user.CodeTimeBlock == null)
                {
                    // Блокировка смены пароля на час
                    user.CodeTimeBlock = DateTime.UtcNow.AddHours(1);

                    // Обновляет данные пользователя
                    await _userManager.UpdateAsync(user);
                }

                return BadRequest(new ValidateError($"Исчерпан лимит попыток. Повторите попытку через час"));
            }

            // Проверяет совпадение двух секретных кодов 
            if (model.CodeInMessage != user.Code)
                return BadRequest(new ValidateError($"Код подтверждения указан неверно. Кол-во попыток: {--model.CountAttempts}"));

            var passwordValidator =
                HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
            var passwordHasher =
                HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

            // Проверяет корректность пароля
            IdentityResult result = await passwordValidator.ValidateAsync(_userManager, user, model.Password);

            // Проверяет, изменен ли пароль пользователя
            if (!result.Succeeded)
                return BadRequest(new ValidateError(result.Errors.ToArray()[0].Description));

            // Удаляет секретный код и блокировку смены пароля
            user.Code = null;
            user.CodeTimeBlock = null;

            // Использование хэш-функции для пароля
            user.PasswordHash = passwordHasher?.HashPassword(user, model.Password);

            // Обновляет данные пользователя
            await _userManager.UpdateAsync(user);

            return Ok();
        }
    }
}
