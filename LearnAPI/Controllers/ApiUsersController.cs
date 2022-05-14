using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<User>> GetUsersAsync() => 
            await _userManager.Users.ToListAsync();

        /// <summary>
        /// Запрос на получение конкретного пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserAsync([FromRoute] string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user != null)
                return Ok(user);

            return NotFound(new ValidateError("Пользователь не найден"));
        }

        /// <summary>
        /// Запрос на добавление нового пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserData model)
        {
            // Создание экземляра класса User
            User user = new User { 
                Email = model.Email, UserName = model.Email,
                Surname = model.Surname, Name = model.Name, 
                LockoutEnabled = model.Enabled
            };

            // Создание нового пользователя
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ValidateError(result.Errors.ToArray()[0].Description));

            // Присваение роли новому пользователю
            await _userManager.AddToRoleAsync(user, "common");

            return Ok();
        }

        /// <summary>
        /// Запрос на изменение данных о пользователе
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync([FromRoute] string id, [FromBody] UserData model)
        {
            // Поиск пользователя по уникальному идентификатору
            User user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return BadRequest(new ValidateError("Пользователь не найден"));

            // Обновляет все возможные поля
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Surname = model.Surname;
            user.Name = model.Name;
            user.LockoutEnabled = model.Enabled;

            // Сохраняет изменения в базе данных
            var result = await _userManager.UpdateAsync(user);

            // Проверяет, вступили ли в силу изменения данных учетной записи
            if (!result.Succeeded)
                return BadRequest(new ValidateError(result.Errors.ToArray()[0].Description));

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            // Поиск пользователя по уникальному идентификатору
            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return BadRequest(new ValidateError("Пользователь не найден"));

            // Удаляет учетную запись пользователя
            var result = await _userManager.DeleteAsync(user);

            // Проверяет, удалена ли учетная запись
            if (!result.Succeeded)
                return BadRequest(new ValidateError(result.Errors.ToArray()[0].Description));

            return Ok();
        }
    }
}
