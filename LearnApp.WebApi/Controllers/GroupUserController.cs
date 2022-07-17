using AutoMapper;
using LearnApp.BLL.Models.Response;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ResponseGroupUserModel>))]
        public async Task<IEnumerable<ResponseGroupUserModel>> GetGroupUsers(Guid groupGuid) =>
            await _service.GetGroupUsersAsync(groupGuid); // todo: протестировать без мапера

        /// <summary>
        /// Запрос на вступление пользователя в конкретную группу
        /// с помощью пригласительного кода
        /// </summary>
        /// <param name="inviteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpPost("{inviteGuid}/{userGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Invite(Guid inviteGuid, Guid userGuid)
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

            return NoContent();
        }

        /// <summary>
        /// Запрос на вступление пользователя в группу,
        /// не требующая пригласительного кода
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpPost("{groupGuid}/{userGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Join(Guid groupGuid, Guid userGuid)
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

            return NoContent();
        }

        /// <summary>
        /// Запрос на выход пользователя из конкретной группы
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpDelete("{groupGuid}/{userGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Leave(Guid groupGuid, Guid userGuid)
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

            return NoContent();
        }
    }
}
