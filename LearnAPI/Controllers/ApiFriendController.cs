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
        public async Task<IEnumerable<User>> GetFriends([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var friends = _repo.GetFriends(user.Id);
            return _mapper.Map<List<User>, List<User>>(friends);
        }

        /// <summary>
        /// Запрос на получение конкретной дружбы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Friend/{id}")]
        public ActionResult<Friend> GetFriend([FromRoute] int id)
        {
            var friend = _repo.GetRecord(id);

            if (friend != null)
                return Ok(friend);

            return NotFound(new List<ValidateError> { new ValidateError("Нет данных о дружбе") });
        }

        /// <summary>
        /// Запрос на создание дружбы
        /// </summary>
        /// <param name="friend"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateFriend([FromBody] Friend friend)
        {
            try
            {
                _repo.Add(friend);
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
        /// Запрос на удаление дружбы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public IActionResult RemoveFriend([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
                timestamp = $"\"{timestamp}\"";

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
