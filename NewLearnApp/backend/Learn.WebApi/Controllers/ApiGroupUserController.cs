using AutoMapper;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiGroupUserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IGroupUserRepo _repo;

        public ApiGroupUserController(IGroupUserRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля GroupUser в объекте User
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<User, User>()
                .ForMember(x => x.GroupUser, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех пользователей, принадлежащих конкретной группе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Group/{id}")]
        public IEnumerable<User> GetGroupUsersAsync([FromRoute] int id) =>
            _mapper.Map<List<User>, List<User>>(_repo.GetGroupUsers(id));

        /// <summary>
        /// Запрос на получение конкретной пользователя группы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{email}/{groupId}")]
        public async Task<ActionResult<GroupRole>> GetUserRoleAsync([FromRoute] string email, [FromRoute] int groupId)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound(new ValidateError("Пользователь не найден"));

            string result = await _repo.UserRoleInGroup(groupId, user.Id);

            if (result == string.Empty)
                return BadRequest(new ValidateError("У вас отсутствует роль в группе"));

            return new GroupRole { Name = result };
        }

        /// <summary>
        /// Запрос на добавление пользователя в конкретную группу
        /// с помощью приглашающего кода
        /// </summary>
        /// <param name="inviteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("Invite/{email}/{inviteId}")]
        public async Task<IActionResult> InviteGroupUserAsync([FromRoute] string email, [FromRoute] string inviteId)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound(new ValidateError("Пользователь не найден"));

            string result = await _repo.AcceptedInviteAsync(inviteId, user.Id);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }

        /// <summary>
        /// Запрос на добавление пользователя в открытую группу
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("Join/{groupId}/{email}")]
        public async Task<IActionResult> JoinGroupUserAsync([FromRoute] int groupId, [FromRoute] string email)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound(new ValidateError("Пользователь не найден"));

            string result = await _repo.JoinOpenGroupAsync(groupId, user.Id);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление пользователя из конкретной группы
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{groupId}/{userId}")]
        public async Task<IActionResult> RemoveGroupUserAsync([FromRoute] int groupId, [FromRoute] string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new ValidateError("Пользователь не найден"));

            string result = await _repo.KickUserAsync(groupId, user.Id);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }
    }
}
