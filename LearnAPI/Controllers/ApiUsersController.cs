using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public ApiUsersController(UserManager<User> userManager) => 
            _userManager = userManager;

        /// <summary>
        /// Запрос на получение всех пользователь
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<User> GetUsers() => 
            _userManager.Users.ToList();

        /// <summary>
        /// Запрос на получение конкретного пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser([FromRoute] string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user != null)
                return Ok(user);

            return NotFound(new List<ValidateError> { new ValidateError("Пользователь не найден") });
        }

        /// <summary>
        /// Запрос на добавление нового пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserData model)
        {
            List<ValidateError>? errors = null;

            // Создание экземляра класса User
            User user = new User { 
                Email = model.Email, UserName = model.Email,
                Surname = model.Surname, Name = model.Name, 
                LockoutEnabled = model.Enabled
            };

            // Создание нового пользователя
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Присваение роли новому пользователю
                await _userManager.AddToRoleAsync(user, "student");

                return Ok();
            }
            else
            {
                // Отправляет ошибки, вызванные отрицательным результатом создания пользователя
                errors = new List<ValidateError>();

                foreach (var error in result.Errors)
                {
                    errors.Add(new ValidateError(error.Description));
                }
            }

            return BadRequest(errors);
        }

        /// <summary>
        /// Запрос на изменение данных о пользователе
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] UserData model)
        {
            List<ValidateError>? errors = null;

            // Поиск пользователя по уникальному идентификатору
            User user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                // Обновляет все возможные поля
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Surname = model.Surname;
                user.Name = model.Name;
                user.LockoutEnabled = model.Enabled;

                // Сохраняет изменения в базе данных
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return Ok();
                else
                {
                    // Отправляет ошибки, вызванные отрицательным результатом обновления пользователя
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

        /// <summary>
        /// Запрос на удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            List<ValidateError>? errors = null;

            // Поиск пользователя по уникальному идентификатору
            User user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Удаляет учетную запись пользователя
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return Ok();
                else
                {
                    // Отправляет ошибки, вызванные отрицательным результатом удаления пользователя
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
