using Microsoft.AspNetCore.Mvc;
using LearnApp.Helper.EmailService;
using LearnApp.BLL.Services;
using LearnApp.BLL.Models.Request;
using LearnApp.WebApi.Services;
using LearnApp.Helper.Logging;
using LearnApp.WebApi.Attributes;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly JwtService _jwtService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AccountService accountService, JwtService jwtService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _jwtService = jwtService;
            _logger = logger;
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
        public async Task<ActionResult> Register(RequestRegisterModel model)
        {
            try
            {
                await _accountService.RegisterAsync(model);
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
        public async Task<ActionResult> Login(RequestLoginModel model)
        {
            (var user, string error) = await _accountService.LoginAsync(model);

            if (user is null || !string.IsNullOrEmpty(error))
                return BadRequest(new { message = error });

            var token = _jwtService.GenerateJwtToken(user.Guid);

            // Для возращения токена в cookie
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                // Доступ к cookie недоступен пользователю,
                // однако будет отправлен в запросе от него автоматически
                HttpOnly = true
            });

            return Ok(new { id = user.Guid });
        }

        /// <summary>
        /// Запрос на выход из учетной записи
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Logout()
        {
            // Удаление токена из cookie
            Response.Cookies.Delete("access_token");

            return await Task.FromResult(NoContent());
        }
    }
}
