using AutoMapper;
using LearnEF.Entities;
using LearnEF.Repos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [Route("api/Source")]
    [ApiController]
    public class SourceLoreController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISourceLoreRepo _repo;

        public SourceLoreController(ISourceLoreRepo repo)
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
        public async Task<IEnumerable<SourceLore>> GetSources()
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
        public async Task<ActionResult<Learn>> GetSource([FromRoute] int id)
        {
            var source = _repo.GetRecord(id);

            if (source == null)
                return NotFound();

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
            _repo.Add(source);
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
            if (id != source.Id)
                return BadRequest();

            _repo.Update(source);
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
                return BadRequest();

            if (!timestamp.StartsWith("\""))
                timestamp = $"\"{timestamp}\"";

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);
            _repo.Delete(id, ts);
            return Ok();
        }
    }
}
