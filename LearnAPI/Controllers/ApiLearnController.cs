using AutoMapper;
using LearnEF.Entities;
using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiLearnController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILearnRepo _repo;

        public ApiLearnController(ILearnRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля SourceLore в объекте Learn
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Learn, Learn>()
                .ForMember(x => x.SourceLore, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех материалов
        /// </summary>
        /// <returns></returns>
        [HttpGet("{email}")]
        public async Task<IEnumerable<Learn>> GetLearns([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var learns = _repo.UserLearns(user.Id);
            return _mapper.Map<List<Learn>, List<Learn>>(learns);
        }

        /// <summary>
        /// Запрос на получение конкретного материала
        /// с учетом вызванного действия и залогиненного пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        [HttpGet("{email}/{id}/{act}")]
        public async Task<ActionResult<Learn>> GetLearn([FromRoute] string email, [FromRoute] int id, [FromRoute] string act)
        {
            // Поиск пользователя и материала
            User user = await _userManager.FindByNameAsync(email);
            var learn = _repo.GetRecord(id);

            if (learn == null)
                return NotFound(new List<ValidateError> { new ValidateError("Материал не найден") });

            // Определяет, какое действие было вызвано
            switch (act)
            {
                case "Details":
                    {
                        // Проверяет, кто пытается запросить данные,
                        // автор или другой пользователь, с кем поделились записью
                        if (!_repo.IsAuthor(learn.Id, user.Id) && !_repo.SharedWith(learn.Id, user.Id))
                            return BadRequest(new List<ValidateError> { new ValidateError("Вы не имеете доступ к данному материалу") });

                        break;
                    }
                case "Edit":
                    {
                        // Проверяет, запрашивает ли автор запись
                        if (!_repo.IsAuthor(learn.Id, user.Id))
                        {
                            // Проверяет, имеет ли доступ к редактированию
                            // данной записи пользователь, с кем ею поделились
                            if (!_repo.CanChangeLearn(learn.Id, user.Id))
                                return BadRequest(new List<ValidateError> { new ValidateError("Вы не имеете доступ к редактированию данного материала") });
                        }    

                        break;
                    }
                case "Delete":
                    {
                        // // Проверяет, запрашивает ли автор запись
                        if (!_repo.IsAuthor(learn.Id, user.Id))
                            return BadRequest(new List<ValidateError> { new ValidateError("Доступ к удалению данного материала имеет только автор") });

                        break;
                    }
                default:
                    return BadRequest(new List<ValidateError> { new ValidateError("Запрос не имеет действия") });
            }

            return Ok(_mapper.Map<Learn, Learn>(learn));
        }

        /// <summary>
        /// Запрос на получение всех ресурсов
        /// </summary>
        /// <returns></returns>
        [HttpGet("sources")]
        public async Task<ActionResult<SourceLore>> GetSources()
        {
            var sources = _repo.GetSourceLores();

            return Ok(_mapper.Map<List<SourceLore>, List<SourceLore>>(sources));
        }

        /// <summary>
        /// Запрос на добавление нового материала
        /// и его привязка к конкретному пользователю
        /// </summary>
        /// <param name="email"></param>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPost("{email}")]
        public async Task<IActionResult> CreateLearn([FromRoute] string email, [FromBody] Learn learn)
        {
            try
            {
                User user = await _userManager.FindByNameAsync(email);
                learn.UserId = user.Id;
                _repo.Add(learn);
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
        /// Запрос на изменение материала
        /// </summary>
        /// <param name="id"></param>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLearn([FromBody] Learn learn)
        {
            try
            {
                _repo.Update(learn);
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
        /// Запрос на удаление материала
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveLearn([FromRoute] int id, [FromRoute] string timestamp)
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
