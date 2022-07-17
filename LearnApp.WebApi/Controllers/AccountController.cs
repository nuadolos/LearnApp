using Microsoft.AspNetCore.Mvc;
using LearnApp.Helper.EmailService;
using LearnApp.BLL.Services;
using LearnApp.WebApi.JWT;
using LearnApp.BLL.Models.Request;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly AccountService _service;

        public AccountController(AccountService service, IConfiguration configuration, IEmailSender emailSender) 
        {
            _service = service;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        /// <summary>
        /// Запрос на регистрацию новой учетной записи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Тест</response>
        /// <response code="400">Тест</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RequestRegisterModel model)
        {
            try
            {
                await _service.RegisterAsync(model);
            }
            catch (Exception ex)
            {
                // add logger
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Запрос на вход в учетную запись
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(RequestLoginModel model)
        {
            try
            {
                var user = await _service.LoginAsync(model);

                // Генерация JWT-токена
                var token = _configuration.GenerateJwtToken(user);

                // Для возращения токена в cookie
                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    // Доступ к cookie недоступен пользователю,
                    // однако будет отправлен в запросе от него автоматически
                    HttpOnly = true
                });

                return Ok(new { id = user.Guid });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Запрос на выход из учетной записи
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            // Удаление токена из cookie
            Response.Cookies.Delete("token");

            return await Task.FromResult(NoContent());
        }
    }
}
