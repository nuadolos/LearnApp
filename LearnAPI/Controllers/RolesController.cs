using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/Roles")]
    [ApiController] 
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Запрос на получение всех возможных ролей из БД
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<IdentityRole> GetRoles() => _roleManager.Roles.ToList();

        /// <summary>
        /// Запрос на создание новой роли
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            List<ValidateError>? errors = null;

            if (!string.IsNullOrEmpty(name))
            {
                //Создает роль в БД
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return Ok();
                else
                {
                    //Получение ошибок, вызванных с неправильно введенными даннами
                    errors = new List<ValidateError>();

                    foreach (var error in result.Errors)
                    {
                        errors.Add(new ValidateError { Message = error.Description });
                    }
                }
            }

            return BadRequest(errors);
        }

        /// <summary>
        /// Запрос на изменение ролей конкретного пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                //Текущий список ролей у пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                
                //Все доступные роли
                var allRoles = _roleManager.Roles.ToList();

                //Роли, которые отмечены флажком
                var addedRoles = roles.Except(userRoles);

                //Роли, которые не отмечены флажком
                var removedRoles = userRoles.Except(roles);

                //Добавление и удаление ролей
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return Ok();
            }

            //Возвращает ошибку 404
            return NotFound();
        }

        /// <summary>
        /// Запрос на удаление роли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                return Ok();
            }

            return BadRequest();
        }
    }
}
