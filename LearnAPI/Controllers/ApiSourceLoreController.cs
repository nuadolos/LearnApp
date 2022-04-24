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
    public class ApiSourceLoreController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISourceLoreRepo _repo;

        public ApiSourceLoreController(ISourceLoreRepo repo)
        {
            _repo = repo;

            //Игнорирование поля Learn в объекте SourceLore
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<SourceLore, SourceLore>()
                .ForMember(x => x.Learn, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех источников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<SourceLore> GetSources()
        {
            var source = _repo.GetAll();
            return _mapper.Map<List<SourceLore>, List<SourceLore>>(source);
        }

        /// <summary>
        /// Запрос на получение конкретного источника
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SourceLore>> GetSource([FromRoute] int id)
        {
            var source = _repo.GetRecord(id);

            if (source == null)
                return NotFound(new List<ValidateError> { new ValidateError("Ресурс не найден") });

            return Ok(_mapper.Map<SourceLore, SourceLore>(source));
        }

        /// <summary>
        /// Запрос на добавление нового источника
        /// </summary>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSource([FromBody] SourceLore source)
        {
            try
            {
                _repo.Add(source);
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
        /// Запрос на изменение источника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSource([FromRoute] int id, [FromBody] SourceLore source)
        {
            try
            {
                _repo.Update(source);
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
        /// Запрос на удаление источника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveSource([FromRoute] int id, [FromRoute] string timestamp)
        {
            //Если у источника есть ссылка хотя бы на один материал
            //то удаление невозможно
            if (_repo.ContainedInLearn(id))
            {
                return BadRequest(new List<ValidateError> {
                    new ValidateError("Удаление невозможно, пока не будут удалены все записи с данным ресурсом") 
                });
            }    

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
