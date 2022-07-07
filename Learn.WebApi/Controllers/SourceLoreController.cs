using AutoMapper;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/source")]
    [ApiController]
    public class SourceLoreController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INoteTypeRepo _repo;

        public SourceLoreController(INoteTypeRepo repo)
        {
            _repo = repo;

            //Игнорирование поля Learn в объекте SourceLore
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<NoteType, NoteType>()
                .ForMember(x => x.Note, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех источников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<NoteType>> GetSourcesAsync() =>
            _mapper.Map<List<NoteType>, List<NoteType>>(await _repo.GetAllAsync());

        /// <summary>
        /// Запрос на получение конкретного источника
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteType>> GetSourceAsync([FromRoute] int id)
        {
            var source = await _repo.GetRecordAsync(id);

            if (source == null)
                return NotFound(new ValidateError("Ресурс не найден"));

            return Ok(source);
        }

        /// <summary>
        /// Запрос на добавление нового источника
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSourceAsync([FromBody] NoteType source)
        {
            try
            {
                await _repo.AddAsync(source);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
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
        public async Task<IActionResult> RemoveSourceAsync([FromRoute] int id, [FromRoute] string timestamp)
        {
            // Если у источника есть ссылка хотя бы на один материал,
            //   то удаление невозможно
            if (_repo.ContainedInNote(id))
            {
                return BadRequest(new ValidateError(
                    "Удаление невозможно, пока не будут удалены все записи с данным ресурсом"));
            }    

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
