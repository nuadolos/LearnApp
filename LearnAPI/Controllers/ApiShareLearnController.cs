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
    public class ApiShareLearnController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapperShareLearn;
        private readonly IMapper _mapperLearn;
        private readonly IMapper _mapperUser;
        private readonly IShareLearnRepo _repo;

        public ApiShareLearnController(IShareLearnRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля ShareLearn в объекте Learn
            var learnConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<Learn, Learn>()
                .ForMember(x => x.ShareLearn, opt => opt.Ignore()));
            _mapperLearn = learnConfig.CreateMapper();

            //Игнорирование поля ShareLearn в объекте User
            var userConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<User, User>()
                .ForMember(x => x.ShareLearn, opt => opt.Ignore()));
            _mapperUser = userConfig.CreateMapper();

            //Игнорирование поля User и Learn в объекте ShareLearn
            var shareLearnConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<ShareLearn, ShareLearn>()
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.Learn, opt => opt.Ignore()));
            _mapperShareLearn = shareLearnConfig.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение материалов пользователем, с кем поделились
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("User/{email}")]
        public async Task<IEnumerable<Learn>> GetLearnsAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var learns = await _repo.GetLearnsAsync(user.Id);
            return _mapperLearn.Map<List<Learn>, List<Learn>>(learns);
        }

        /// <summary>
        /// Запрос на получение пользователей, 
        /// с кеми поделились конкретным материалом
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Learn/{id}")]
        public async Task<IEnumerable<User>> GetUsersAsync([FromRoute] int id) =>
            _mapperUser.Map<List<User>, List<User>>(await _repo.GetUsersAsync(id));

        /// <summary>
        /// Запрос на получение конкретной записи открытния доступа к материалу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ShareLearn>> GetShareAsync([FromRoute] int id)
        {
            var share = await _repo.GetRecordAsync(id);

            if (share != null)
                return Ok(share);

            return NotFound(new List<ValidateError> { new ValidateError("Нет данных о открытии доступа к материалу") });
        }

        /// <summary>
        /// Запрос на создание открытого доступа к материалу
        /// </summary>
        /// <param name="shapeLearn"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateShareAsync([FromBody] ShareLearn shapeLearn)
        {
            try
            {
                await _repo.AddAsync(shapeLearn);
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
        /// Запрос на удаление открытого доступа к материалу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveShareAsync([FromRoute] int id, [FromRoute] string timestamp)
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
