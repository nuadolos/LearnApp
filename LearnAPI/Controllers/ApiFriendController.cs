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
    public class ApiFriendController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IFriendRepo _repo;

        public ApiFriendController(IFriendRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля SentUser и AcceptedUser в объекте Friend
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<User, User>()
                .ForMember(x => x.SentUser, opt => opt.Ignore())
                .ForMember(x => x.AcceptedUser, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех друзей конкретного пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("Friends/{email}")]
        public async Task<IEnumerable<User>> GetFriendsAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var friends = await _repo.GetFriendsAsync(user.Id);
            return _mapper.Map<List<User>, List<User>>(friends);
        }

        /// <summary>
        /// Запрос на получение конкретной дружбы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Friend/{id}")]
        public async Task<ActionResult<Friend>> GetFriendAsync([FromRoute] int id)
        {
            var friend = await _repo.GetRecordAsync(id);

            if (friend != null)
                return Ok(friend);

            return NotFound(new ValidateError("Нет данных о дружбе"));
        }

        /// <summary>
        /// Запрос на создание дружбы
        /// </summary>
        /// <param name="friend"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateFriendAsync([FromBody] Friend friend)
        {
            try
            {
                await _repo.AddAsync(friend);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление дружбы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveFriendAsync([FromRoute] int id, [FromRoute] string timestamp)
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
