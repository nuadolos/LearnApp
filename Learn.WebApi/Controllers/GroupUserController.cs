using AutoMapper;
using LearnApp.BLL.Models.Response;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupUserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly GroupUserService _service;

        public GroupUserController(GroupUserService service)
        {
            _service = service;

            //Игнорирование ссылочного поля в объекте User
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<User, User>()
                .ForMember(x => x.GroupUsers, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех пользователей, принадлежащих конкретной группе
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <returns></returns>
        [HttpGet("{groupGuid}")]
        public async Task<IEnumerable<ResponseGroupUserModel>> GetGroupUsersAsync(Guid groupGuid) =>
            await _service.GetGroupUsersAsync(groupGuid); // todo: протестировать без мапера

        /// <summary>
        /// Запрос на вступление пользователя в конкретную группу
        /// с помощью пригласительного кода
        /// </summary>
        /// <param name="inviteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpPost("Invite/{inviteGuid}/{userGuid}")]
        public async Task<IActionResult> InviteGroupUserAsync(Guid inviteGuid, Guid userGuid)
        {
            try
            {
                await _service.JoinGroupByInviteCodeAsync(inviteGuid, userGuid);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на вступление пользователя в группу,
        /// не требующая пригласительного кода
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpPost("Join/{groupGuid}/{userGuid}")]
        public async Task<IActionResult> JoinGroupUserAsync(Guid groupGuid, Guid userGuid)
        {
            try
            {
                await _service.JoinGroupByGuidAsync(groupGuid, userGuid);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на выход пользователя из конкретной группы
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpDelete("{groupGuid}/{userGuid}")]
        public async Task<IActionResult> RemoveGroupUserAsync(Guid groupGuid, Guid userGuid)
        {
            try
            {
                await _service.LeaveGroupAsync(groupGuid, userGuid);
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
