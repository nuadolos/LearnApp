using AutoMapper;
using LearnEF.Entities;
using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiGroupUserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGroupUserRepo _repo;

        public ApiGroupUserController(IGroupUserRepo repo)
        {
            _repo = repo;

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
        public async Task<IEnumerable<User>> GetGroupUsersAsync([FromRoute] int id) =>
            _mapper.Map<List<User>, List<User>>(await _repo.GetGroupUsers(id));

        /// <summary>
        /// Запрос на получение конкретной записи пользователя группы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupUser>> GetGroupUserAsync([FromRoute] int id)
        {
            var groupUser = await _repo.GetRecordAsync(id);

            if (groupUser != null)
                return Ok(groupUser);

            return NotFound(new ValidateError("Нет данных о пользователе, находящийся в конкретной группе"));
        }

        /// <summary>
        /// Запрос на добавление пользователя в конкретную группу
        /// с помощью приглашающего кода
        /// </summary>
        /// <param name="inviteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("i/{inviteId}/u/{userId}")]
        public async Task<IActionResult> InviteGroupUserAsync([FromRoute] string inviteId, [FromRoute] string userId)
        {
            string result = await _repo.AcceptedInviteAsync(inviteId, userId);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }

        /// <summary>
        /// Запрос на добавление пользователя в конкретную группу
        /// </summary>
        /// <param name="groupUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateGroupUserAsync([FromBody] GroupUser groupUser)
        {
            try
            {
                await _repo.AddAsync(groupUser);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

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
