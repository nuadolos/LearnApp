using AutoMapper;
using LearnEF.Entities;
using LearnEF.Entities.ErrorModel;
using LearnEF.Repos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiLearnController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILearnRepo _repo;

        public ApiLearnController(ILearnRepo repo)
        {
            _repo = repo;

            //Игнорирование поля SourceLore в объекте Learn
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Learn, Learn>()
                .ForMember(x => x.SourceLore, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех материалов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Learn>> GetLearns()
        {
            var learns = _repo.GetAll();
            return _mapper.Map<List<Learn>, List<Learn>>(learns);
        }

        /// <summary>
        /// Запрос на получение конкретного материала
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Learn>> GetLearn([FromRoute] int id)
        {
            var learn = _repo.GetRecord(id);

            if (learn == null)
                return NotFound(new List<ValidateError> { new ValidateError("Материал не найден") });

            return Ok(_mapper.Map<Learn, Learn>(learn));
        }

        [HttpGet("sources")]
        public async Task<ActionResult<SourceLore>> GetSources()
        {
            var sources = _repo.GetSourceLores();

            return Ok(_mapper.Map<List<SourceLore>, List<SourceLore>>(sources));
        }

        /// <summary>
        /// Запрос на добавление нового материала
        /// </summary>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateLearn([FromBody] Learn learn)
        {
            try
            {
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
