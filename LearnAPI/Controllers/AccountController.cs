using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/AccountController")]
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

        #region Регистрация

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegister model)
        {
            List<ValidateError>? errors = null;

            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    //return RedirectToAction();
                    return Ok();
                }
                else
                {
                    errors = new List<ValidateError>();

                    foreach (var error in result.Errors)
                    {
                        errors.Add(new ValidateError { Message = error.Description });
                    }
                }
            }

            return BadRequest(errors);
        }

        #endregion

        #region Авторизация

        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string? returnUrl = null) => View(new UserLogin { ReturnUrl = returnUrl });

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            ValidateError? error = null;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                    //return !string.IsNullOrEmpty(model.ReturnUrl) ? Redirect(model.ReturnUrl) : RedirectToAction("", "");
                    return Ok();
            }
            else
                error = new ValidateError { Message = "Неправельный логин или пароль" };

            return BadRequest(error);
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        #endregion
    }
}
