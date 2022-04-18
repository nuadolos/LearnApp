using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearnAPI.Controllers
{
    [Route("api/UsersController")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager) => _userManager = userManager;

        #region Получение данных о пользователе(-ях)

        [HttpGet]
        public IEnumerable<User> GetUsers() => _userManager.Users.ToList();

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user != null)
                return Json(user);

            return NotFound();

        }

        #endregion

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserData model)
        {
            List<ValidateError>? validateErrors = null;

            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                    return Ok();
                else
                {
                    validateErrors = new List<ValidateError>();

                    foreach (var error in result.Errors)
                    {
                        validateErrors.Add(new ValidateError { Message = error.Description });
                    }
                }
            }

            return BadRequest(validateErrors);
        }

        /// <summary>
        /// Изменение данных о пользователе
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] UserData model)
        {
            List<ValidateError>? errors = null;

            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return Ok();
                    else
                    {
                        errors = new List<ValidateError>();

                        foreach (var error in result.Errors)
                        {
                            errors.Add(new ValidateError { Message = error.Description });
                        }
                    }
                }
            }

            return BadRequest(errors);
        }
        
        #region Восстановление пароля

        [HttpPost]
        [Route("changed={id}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string id, [FromBody] UserData model)
        {
            List<ValidateError>? errors = null;

            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(id);

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

                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

                    if (result.Succeeded)
                        return Ok();
                    else
                    {
                        errors = new List<ValidateError>();

                        foreach (var error in result.Errors)
                        {
                            errors.Add(new ValidateError { Message = error.Description });
                        }
                    }
                }
            }

            return BadRequest(errors);
        }

        #endregion
    }
}
