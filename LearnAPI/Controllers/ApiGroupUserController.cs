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
        public IEnumerable<User> GetGroupUsers([FromRoute] int id) =>
            _mapper.Map<List<User>, List<User>>(_repo.GetGroupUsers(id));

        /// <summary>
        /// Запрос на получение конкретной записи пользователя группы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<GroupUser> GetGroupUser([FromRoute] int id)
        {
            var groupUser = _repo.GetRecord(id);

            if (groupUser != null)
                return Ok(groupUser);

            return NotFound(new List<ValidateError> {
                new ValidateError("Нет данных о пользователе, находящийся в конкретной группе")
            });
        }

        /// <summary>
        /// Запрос на добавление пользователя в конкретную группу
        /// </summary>
        /// <param name="groupUser"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateGroupUser([FromBody] GroupUser groupUser)
        {
            try
            {
                _repo.Add(groupUser);
            }
            catch (DbMessageException ex)
            {
                //Получение ошибок при создании записи
                List<ValidateError> errors = new List<ValidateError>();

                errors.Add(new ValidateError(ex.Message));

                return BadRequest(errors);
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
        public IActionResult RemoveGroupUser([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
                timestamp = $"\"{timestamp}\"";

            if (timestamp.Contains("%2F"))
                timestamp = timestamp.Replace("%2F", "/");

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);

            try
            {
                _repo.Delete(id, ts);
            }
            catch (DbMessageException ex)
            {
                //Получение ошибок при создании записи
                List<ValidateError> errors = new List<ValidateError>();

                errors.Add(new ValidateError(ex.Message));

                return BadRequest(errors);
            }

            return Ok();
        }
    }
}
