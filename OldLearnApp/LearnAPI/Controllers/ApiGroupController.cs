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

            //Игнорирование поля GroupType, GroupUser и User в объекте Group
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Group, Group>()
                .ForMember(x => x.GroupType, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.GroupUser, opt => opt.Ignore()));

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех групп
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Group>> GetGroupsAsync() =>
            _mapper.Map<List<Group>, List<Group>>(await _repo.GetVisibleGroupsAsync());

        /// <summary>
        /// Запрос на получение собственных групп пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("MyGroup/{email}")]
        public async Task<IEnumerable<Group>> GetUserGroupsAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            var groups = await _repo.GetUserGroupsAsync(user.Id);
            return _mapper.Map<List<Group>, List<Group>>(groups);
        }

        /// <summary>
        /// Запрос на получение групп, где пользователь является участником
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("Member/{email}")]
        public async Task<IEnumerable<Group>> GetMemberGroupsAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            var groups = await _repo.GetMemberGroupsAsync(user.Id);
            return _mapper.Map<List<Group>, List<Group>>(groups);
        }

        /// <summary>
        /// Запрос на получение группы конкретным пользователем
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{email}/{id}/{act}")]
        public async Task<ActionResult<Group>> GetGroupAsync([FromRoute] string email, [FromRoute] int id, [FromRoute] string act)
        {
            User user = await _userManager.FindByEmailAsync(email);
            var group = await _repo.GetRecordAsync(id);

            if (group == null)
                return NotFound(new ValidateError("Группа не найдена"));

            switch (act)
            {
                case "Details":
                {
                    // Проверяет доступность к группе
                    if (!group.IsVisible)
                    {
                        // Проверяет, является ли пользователь участником группы
                        if (!await _repo.IsCreatorAsync(group.Id, user.Id) && !await _repo.IsMemberAsync(group.Id, user.Id))
                            return BadRequest(new ValidateError("Вы не являетесь участником данной группы"));
                    }

                    break;
                }

                case "Edit":
                case "Delete":
                {
                    // Проверяет, является ли пользователь создателем группы
                    if (!await _repo.IsCreatorAsync(group.Id, user.Id))
                        return BadRequest(new ValidateError($"{(act == "Edit" ? "Редактирование" : "Удаление")}" +
                            $" может осуществить только создатель группы"));

                    break;
                }

                default:
                    return BadRequest(new ValidateError("Запрос не имеет действия"));
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

                // Если группа будет закрытой,
                //   то создается пригласительный код для набора пользователей
                if (!group.IsVisible)
                    group.CodeInvite = Guid.NewGuid().ToString();

                await _repo.AddAsync(group);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
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
                if (!group.IsVisible)
                    group.CodeInvite = Guid.NewGuid().ToString();
                else
                    group.CodeInvite = null;

                await _repo.UpdateAsync(group);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление группы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveGroupAsync([FromRoute] int id)
        {
            string result = await _repo.DeleteAllDataAboutGroupAsync(id);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }
    }
}
