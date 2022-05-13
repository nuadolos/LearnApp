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
    public class ApiGroupController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IGroupRepo _repo;

        public ApiGroupController(IGroupRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля GroupType, GroupUser и GroupLearn в объекте Group
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Group, Group>()
                .ForMember(x => x.GroupType, opt => opt.Ignore())
                .ForMember(x => x.GroupUser, opt => opt.Ignore())
                .ForMember(x => x.GroupLearn, opt => opt.Ignore()));

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех групп
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            var groups = await _repo.GetVisibleGroupsAsync();
            return _mapper.Map<List<Group>, List<Group>>(groups);
        }

        /// <summary>
        /// Запрос на получение группы конкретным пользователем
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/{email}")]
        public async Task<ActionResult<Group>> GetGroupAsync([FromRoute] int id, [FromRoute] string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            var group = await _repo.GetRecordAsync(id);

            if (group == null)
                return NotFound(new List<ValidateError> { new ValidateError("Группа не найдена") });

            if (!group.IsVisible)
            {
                if (!await _repo.IsMemberAsync(group.Id, user.Id))
                    return BadRequest(new List<ValidateError> { new ValidateError("Вы не являетесь участником данной группы") });
            }

            return Ok(_mapper.Map<Group, Group>(group));
        }

        /// <summary>
        /// Запрос на создание группы
        /// </summary>
        /// <param name="email"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost("{email}")]
        public async Task<IActionResult> CreateGroupAsync([FromRoute] string email, [FromBody] Group group)
        {
            try
            {
                User user = await _userManager.FindByNameAsync(email);
                group.UserId = user.Id;
                group.CreateDate = DateTime.Now;
                await _repo.AddAsync(group);
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
        /// Запрос на изменение группы
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroupAsync([FromBody] Group group)
        {
            try
            {
                await _repo.UpdateAsync(group);
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
        /// Запрос на удаление группы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveGroupAsync([FromRoute] int id, [FromRoute] string timestamp)
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
                //Получение ошибок при создании записи
                List<ValidateError> errors = new List<ValidateError>();

                errors.Add(new ValidateError(ex.Message));

                return BadRequest(errors);
            }

            return Ok();
        }
    }
}
