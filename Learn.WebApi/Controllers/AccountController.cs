using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LearnApp.DAL.Entities;
using LearnApp.Helper.EmailService;
using LearnApp.DAL.Repos.IRepos;
using LearnApp.BL.Services;
using LearnApp.BL.Models;
using LearnApp.WebApi.JWT;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/account")]
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
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RequestRegisterModel model)
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

            return Ok();
        }

        /// <summary>
        /// Запрос на вход в учетную запись
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(RequestLoginModel model)
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

        /// <summary>
        /// Запрос на выход из учетной записи
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            // Удаление токена из cookie
            Response.Cookies.Delete("token");

            return await Task.FromResult(Ok());
        }
    }
}
