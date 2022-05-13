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
    public class ApiLearnController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILearnRepo _repo;

        public ApiLearnController(ILearnRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование ссылочных полей объекта Learn
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Learn, Learn>()
                .ForMember(x => x.SourceLore, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.ShareLearn, opt => opt.Ignore())
                .ForMember(x => x.GroupLearn, opt => opt.Ignore())
                .ForMember(x => x.LearnDocuments, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        #region Пользовательские запросы на материал

        /// <summary>
        /// Запрос на получение всех материалов конкретного пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("User/{email}")]
        public async Task<IEnumerable<Learn>> GetUserLearnsAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var learns = await _repo.UserLearnsAsync(user.Id);
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
        public async Task<ActionResult<Learn>> GetUserLearnAsync([FromRoute] string email, [FromRoute] int id, [FromRoute] string act)
        {
            // Поиск пользователя и материала
            User user = await _userManager.FindByNameAsync(email);
            var learn = await _repo.GetRecordAsync(id);

            if (learn == null)
                return NotFound(new List<ValidateError> { new ValidateError("Материал не найден") });

            // Определяет, какое действие было вызвано
            switch (act)
            {
                case "Details":
                    {
                        // Проверяет, кто пытается запросить данные,
                        // автор или другой пользователь, с кем поделились записью
                        if (!await _repo.IsAuthorAsync(learn.Id, user.Id) && !await _repo.SharedWithAsync(learn.Id, user.Id))
                            return BadRequest(new List<ValidateError> { new ValidateError("Вы не имеете доступ к данному материалу") });

                        break;
                    }
                case "Edit":
                    {
                        // Проверяет, запрашивает ли автор запись
                        if (!await _repo.IsAuthorAsync(learn.Id, user.Id))
                        {
                            // Проверяет, имеет ли доступ к редактированию
                            // данной записи пользователь, с кем ею поделились
                            if (!await _repo.CanChangeLearnAsync(learn.Id, user.Id))
                                return BadRequest(new List<ValidateError> { new ValidateError("Вы не имеете доступ к редактированию данного материала") });
                        }    

                        break;
                    }
                case "Delete":
                    {
                        // Проверяет, запрашивает ли автор запись
                        if (!await _repo.IsAuthorAsync(learn.Id, user.Id))
                            return BadRequest(new List<ValidateError> { new ValidateError("Доступ к удалению данного материала имеет только автор") });

                        break;
                    }
                default:
                    return BadRequest(new List<ValidateError> { new ValidateError("Запрос не имеет действия") });
            }

            return Ok(_mapper.Map<Learn, Learn>(learn));
        }

        #endregion

        #region Групповые запросы на материал

        /// <summary>
        /// Запрос на получение всех материалов конкретной группы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Group/{id}")]
        public async Task<IEnumerable<Learn>> GetGroupLearnsAsync([FromRoute] int id)
        {
            var learns = await _repo.GroupLearnsAsync(id);
            return _mapper.Map<List<Learn>, List<Learn>>(learns);
        }

        /// <summary>
        /// Запрос на получение конкретного материала
        /// конкретной группы с учетом
        /// вызванного действия и залогиненного пользователя
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="email"></param>
        /// <param name="learnId"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        [HttpGet("{groupId}/{email}/{learnId}/{act}")]
        public async Task<ActionResult<Learn>> GetGroupLearnAsync([FromRoute] int groupId, [FromRoute] string email, [FromRoute] int learnId, [FromRoute] string act)
        {
            // Поиск пользователя и материала
            User user = await _userManager.FindByNameAsync(email);
            var learn = await _repo.GetRecordAsync(learnId);

            if (learn == null || await _repo.GroupIsNullAsync(groupId))
                return NotFound(new List<ValidateError> { new ValidateError("Материал не найден") });

            // Определяет, какое действие было вызвано
            switch (act)
            {
                case "Details":
                    {
                        // Проверяет, участник ли группы пытается запросить данные
                        if (!await _repo.IsCreaterAsync(groupId, user.Id) && !await _repo.IsMemberGroupAsync(learn.Id, groupId, user.Id))
                            return BadRequest(new List<ValidateError> { new ValidateError("Вы не имеете доступ к данному материалу") });

                        break;
                    }
                case "Edit":
                    {
                        // Проверяет, запрашивает ли создатель группы
                        if (!await _repo.IsCreaterAsync(groupId, user.Id))
                        {
                            // Проверяет, имеет ли доступ к редактированию
                            // данной записи участник группы
                            if (!await _repo.CanChangeLearnAsync(learn.Id, groupId, user.Id))
                                return BadRequest(new List<ValidateError> { new ValidateError("Вы не имеете доступ к редактированию данного материала") });
                        }

                        break;
                    }
                case "Delete":
                    {
                        // Проверяет, запрашивает ли автор запись
                        if (!await _repo.IsCreaterAsync(groupId, user.Id) && !await _repo.IsAuthorAsync(learn.Id, user.Id))
                            return BadRequest(new List<ValidateError> { 
                                new ValidateError("Доступ к удалению данного материала имеет только автор или создатель группы") 
                            });

                        break;
                    }
                default:
                    return BadRequest(new List<ValidateError> { new ValidateError("Запрос не имеет действия") });
            }

            return Ok(_mapper.Map<Learn, Learn>(learn));
        }

        #endregion

        /// <summary>
        /// Запрос на получение всех ресурсов
        /// </summary>
        /// <returns></returns>
        [HttpGet("sources")]
        public async Task<ActionResult<SourceLore>> GetSourcesAsync()
        {
            var sources = await _repo.GetSourceLoresAsync();

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
        public async Task<IActionResult> CreateLearnAsync([FromRoute] string email, [FromBody] Learn learn)
        {
            try
            {
                User user = await _userManager.FindByNameAsync(email);
                learn.UserId = user.Id;
                await _repo.AddAsync(learn);
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
        public async Task<IActionResult> UpdateLearnAsync([FromBody] Learn learn)
        {
            try
            {
                await _repo.UpdateAsync(learn);
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
        public async Task<IActionResult> RemoveLearnAsync([FromRoute] int id, [FromRoute] string timestamp)
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
