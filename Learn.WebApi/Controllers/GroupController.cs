using AutoMapper;
using LearnApp.BLL.Models;
using LearnApp.BLL.Models.Create;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/group")]
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
        public async Task<IEnumerable<Group>> GetGroupsAsync() =>
            _mapper.Map<List<Group>, List<Group>>(await _service.GetVisibleGroupsAsync());

        /// <summary>
        /// Запрос на получение всех групп,
        /// в которых состоит пользователь
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet("{userGuid}")]
        public async Task<IEnumerable<Group>> GetUserGroupsAsync(Guid userGuid) =>
            _mapper.Map<List<Group>, List<Group>>(await _service.GetUserGroupsAsync(userGuid));

        /// <summary>
        /// Запрос на получение деталей группы конкретным пользователем
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet("{groupGuid}/{userGuid}")]
        public async Task<IActionResult> GetGroupAsync(Guid groupGuid, Guid userGuid)
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
        public async Task<IActionResult> CreateGroupAsync(RequestGroupModel model)
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
        public async Task<IActionResult> UpdateGroupAsync(Guid groupGuid, RequestGroupModel model)
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

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление группы ее создателем
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveGroupAsync(RequestRemoveDataModel model)
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

            return Ok();
        }
    }
}
