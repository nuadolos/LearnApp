using AutoMapper;
using LearnApp.BLL.Models.Request;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly GroupService _service;

        public GroupController(GroupService service)
        {
            _service = service;

            //Игнорирование поля ссылочных объектов в Group
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Group, Group>()
                .ForMember(x => x.GroupType, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.GroupUsers, opt => opt.Ignore()));

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение открытых групп
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Group>))]
        public async Task<ActionResult<IEnumerable<Group>>> GetVisibleGroups() =>
            _mapper.Map<List<Group>, List<Group>>(await _service.GetVisibleGroupsAsync());

        /// <summary>
        /// Запрос на получение всех групп,
        /// в которых состоит пользователь
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet("{userGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Group>))]
        public async Task<ActionResult<IEnumerable<Group>>> GetUserGroups(Guid userGuid) =>
            _mapper.Map<List<Group>, List<Group>>(await _service.GetUserGroupsAsync(userGuid));

        /// <summary>
        /// Запрос на получение деталей группы конкретным пользователем
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet("{groupGuid}/{userGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Group>> GetGroup(Guid groupGuid, Guid userGuid)
        {
            try
            {
                var group = await _service.GetGroupAsync(groupGuid, userGuid);
                return Ok(_mapper.Map<Group, Group>(group));
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Запрос на создание группы конкретным пользователем
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Group>> CreateGroup(RequestGroupModel model)
        {
            try
            {
                var group = await _service.CreateGroupAsync(model);
                return Ok(_mapper.Map<Group, Group>(group));
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Запрос на изменение данных о группе ее создателем
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{groupGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateGroup(Guid groupGuid, RequestGroupModel model)
        {
            try
            {
                await _service.UpdateGroupAsync(groupGuid, model);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Запрос на удаление группы ее создателем
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveGroup(RequestRemoveDataModel model)
        {
            try
            {
                await _service.RemoveGroupAsync(model);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
