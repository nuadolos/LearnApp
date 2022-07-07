using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LearnApp.DAL.Entities.IdentityModel;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.WebApi.Utils;
using LearnApp.WebApi.Helper;
using LearnApp.Helper.EmailService;
using LearnApp.DAL.Repos.IRepos;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepo _repo;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AccountController(IUserRepo repo, IConfiguration configuration, IEmailSender emailSender) 
        {
            _repo = repo;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        /// <summary>
        /// Запрос на регистрацию новой учетной записи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistModel model)
        {
            User user = new User {
                Id = Guid.NewGuid().ToString(),
                Login = model.Login,
                Surname = model.Surname,
                Name = model.Name
            };

            user.PasswordHash = PasswordSecurity.PasswordHashing(model.Password, out string salt);
            user.Salt = salt;

            try
            {
                await _repo.AddAsync(user);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на вход в учетную запись
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            var user = await _repo.GetByLoginAsync(model.Login);

            if (user == null)
                return BadRequest(new ValidateError("Пользователь не найден"));

            // Проверка паролей
            if (!PasswordSecurity.CheckPassword(user, model.Password))
                return BadRequest(new ValidateError("Пароль введен неверно"));

            // Генерация JWT-токена
            var token = _configuration.GenerateJwtToken(user);

            // Для возращения токена в cookie
            Response.Cookies.Append("token", token, new CookieOptions
            {
                // Доступ к cookie недоступен пользователю,
                // однако будет отправлен в запросе от него автоматически
                HttpOnly = true
            });

            return Ok(new { id = user.Id });
        }

        /// <summary>
        /// Запрос на выход из учетной записи
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Удаление токена из cookie
            Response.Cookies.Delete("token");

            return await Task.FromResult(Ok());
        }

        [HttpGet]
        [Authorize(Role = "Владимир")]
        public async Task<IEnumerable<User>> GetInfoAsync() => 
            await _repo.GetAllAsync();
    }
}
