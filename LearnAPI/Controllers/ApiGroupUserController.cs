using AutoMapper;
using LearnEF.Entities;
using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
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
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupLearn>> GetGroupUserAsync([FromRoute] int id)
        {
            var groupUser = await _repo.GetRecordAsync(id);

            if (groupUser != null)
                return Ok(groupUser);

            return NotFound(new ValidateError("Нет данных о пользователе конкретной группы"));
        }

        /// <summary>
        /// Запрос на добавление пользователя в конкретную группу
        /// с помощью приглашающего кода
        /// </summary>
        /// <param name="inviteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("{email}/{inviteId}")]
        public async Task<IActionResult> InviteGroupUserAsync([FromRoute] string email, [FromRoute] string inviteId)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound("Пользователь не найден");

            string result = await _repo.AcceptedInviteAsync(inviteId, user.Id);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление пользователя из конкретной группы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveGroupUserAsync([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
                timestamp = $"\"{timestamp}\"";

            if (timestamp.Contains("%2F"))
                timestamp = timestamp.Replace("%2F", "/");

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);

            try
            {
                await _repo.DeleteAsync(id, ts);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }
    }
}
