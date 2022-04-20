using AutoMapper;
using LearnEF.Entities;
using LearnEF.Repos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [Route("api/Learn")]
    [ApiController]
    public class LearnController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILearnRepo _repo;

        public LearnController(ILearnRepo repo)
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
                return NotFound();

            return Ok(_mapper.Map<Learn, Learn>(learn));
        }

        /// <summary>
        /// Запрос на добавление нового материала
        /// </summary>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateLearn([FromBody] Learn learn)
        {
            _repo.Add(learn);
            return Ok();
        }

        /// <summary>
        /// Запрос на изменение материала
        /// </summary>
        /// <param name="id"></param>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLearn([FromRoute] int id, [FromBody] Learn learn)
        {
            if (id != learn.Id)
                return BadRequest();

            _repo.Update(learn);
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
            _repo.Delete(id, ts);
            return Ok();
        }
    }
}
