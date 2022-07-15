using AutoMapper;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Mvc;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly FollowerService _service;

        public FollowerController(FollowerService service)
        {
            _service = service;

            //Игнорирование ссылочные поля в объекте User
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<User, User>()
                .ForMember(x => x.SubscribeUsers, opt => opt.Ignore())
                .ForMember(x => x.TrackedUsers, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех отслеживаемых пользователей
        /// </summary>
        /// <param name="subUserGuid"></param>
        /// <returns></returns>
        [HttpGet("following/{subUserGuid}")]
        public async Task<IEnumerable<User>> GetFollowingAsync(Guid subUserGuid) =>
            _mapper.Map<List<User>, List<User>>(await _service.GetFollowingAsync(subUserGuid));

        /// <summary>
        /// Запрос на получение всех подписчиков пользователя
        /// </summary>
        /// <param name="trackUserGuid"></param>
        /// <returns></returns>
        [HttpGet("followers/{trackUserGuid}")]
        public async Task<IEnumerable<User>> GetFollowersAsync(Guid trackUserGuid) =>
            _mapper.Map<List<User>, List<User>>(await _service.GetFollowersAsync(trackUserGuid));

        /// <summary>
        /// Запрос на оформление подписки на конкретного пользователя
        /// </summary>
        /// <param name="subUserGuid"></param>
        /// <param name="trackUserGuid"></param>
        /// <returns></returns>
        [HttpPost("{subUserGuid}/{trackUserGuid}")]
        public async Task<IActionResult> FollowAsync(Guid subUserGuid, Guid trackUserGuid)
        {
            try
            {
                await _service.FollowAsync(subUserGuid, trackUserGuid);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на прекращение отслеживания за конкретним пользователем
        /// </summary>
        /// <param name="subUserGuid"></param>
        /// <param name="trackUserGuid"></param>
        /// <returns></returns>
        [HttpDelete("{subUserGuid}/{trackUserGuid}")]
        public async Task<IActionResult> UnfollowAsync(Guid subUserGuid, Guid trackUserGuid)
        {
            try
            {
                await _service.UnfollowAsync(subUserGuid, trackUserGuid);
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
