using AutoMapper;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiFollowController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IFollowRepo _repo;

        public ApiFollowController(IFollowRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля SubscribeUser и TrackedUser в объекте Friend
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<User, User>()
                .ForMember(x => x.SubscribeUser, opt => opt.Ignore())
                .ForMember(x => x.TrackedUser, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех отслеживающих пользователей
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("Following/{email}")]
        public async Task<IEnumerable<User>> GetFollowingAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var following = await _repo.GetFollowingAsync(user.Id);
            return _mapper.Map<List<User>, List<User>>(following);
        }

        /// <summary>
        /// Запрос на получение всех подписчиков пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("Followers/{email}")]
        public async Task<IEnumerable<User>> GetFollowersAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var followers = await _repo.GetFollowersAsync(user.Id);
            return _mapper.Map<List<User>, List<User>>(followers);
        }

        /// <summary>
        /// Запрос на оформление подписки на конкретного пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("{email}/{userId}")]
        public async Task<IActionResult> FollowAsync([FromRoute] string email, [FromRoute] string userId)
        {
            User user = await _userManager.FindByEmailAsync(email);
            User findUser = await _userManager.FindByIdAsync(userId);

            if (findUser == null)
                return NotFound(new ValidateError("Искомый пользователь не найден"));

            string result = await _repo.FollowAsync(user.Id, findUser.Id);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }

        /// <summary>
        /// Запрос на прекращение отслеживания за конкретним пользователем
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{email}/{userId}")]
        public async Task<IActionResult> RemoveFriendAsync([FromRoute] string email, [FromRoute] string userId)
        {
            User user = await _userManager.FindByEmailAsync(email);
            User findUser = await _userManager.FindByIdAsync(userId);

            if (findUser == null)
                return NotFound(new ValidateError("Искомый пользователь не найден"));

            string result = await _repo.UnfollowAsync(user.Id, findUser.Id);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }
    }
}
