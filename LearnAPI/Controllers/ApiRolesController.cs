﻿using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class ApiRolesController : ControllerBase
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public ApiRolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Запрос на получение всех возможных ролей из БД
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<IdentityRole> GetRoles() =>
            _roleManager.Roles.ToList();

        /// <summary>
        /// Запрос на получение роли из БД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityRole>> GetRole([FromRoute] string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            if (role != null)
                return Ok(role);

            return NotFound(new List<ValidateError> { new ValidateError("Роль не найдена") });
        }

        /// <summary>
        /// Запрос на создание новой роли
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            List<ValidateError>? errors = null;

            //Создает роль в БД
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(role.Name));
            if (result.Succeeded)
                return Ok();
            else
            {
                //Получение ошибок, вызванных с неправильно введенными данными
                errors = new List<ValidateError>();

                foreach (var error in result.Errors)
                {
                    errors.Add(new ValidateError(error.Description));
                }
            }

            return BadRequest(errors);
        }

        /// <summary>
        /// Запрос на заполнение данных модели UserRoles
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("User/{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                UserRoles model = new UserRoles
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UsRoles = userRoles,
                    AllRoles = allRoles
                };
                return Ok(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Запрос на изменение ролей конкретного пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] IList<string> roles)
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
            return NotFound(new List<ValidateError> { new ValidateError("Пользователь не найден") });
        }

        /// <summary>
        /// Запрос на удаление роли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] IdentityRole model)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(model.Id);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                return Ok();
            }

            return BadRequest(new List<ValidateError> { new ValidateError("Данные о роли отсутствуют") });
        }
    }
}
