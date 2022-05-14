using AutoMapper;
using LearnEF.Entities;
using LearnEF.Entities.ErrorModel;
using LearnEF.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiGroupLearnController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGroupLearnRepo _repo;

        public ApiGroupLearnController(IGroupLearnRepo repo)
        {
            _repo = repo;

            //Игнорирование поля GroupLearn в объекте Learn
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Learn, Learn>()
                .ForMember(x => x.GroupLearn, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех материалов, принадлежащих конкретной группе
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Group/{id}")]
        public async Task<IEnumerable<Learn>> GetGroupLearnsAsync([FromRoute] int id) =>
            _mapper.Map<List<Learn>, List<Learn>>(await _repo.GetGroupLearnsAsync(id));

        /// <summary>
        /// Запрос на получение конкретной записи материала группы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupLearn>> GetGroupLearnAsync([FromRoute] int id)
        {
            var groupLearn = await _repo.GetRecordAsync(id);

            if (groupLearn != null)
                return Ok(groupLearn);

            return NotFound(new ValidateError("Нет данных о материале, принадлежащий конкретной группе"));
        }

        /// <summary>
        /// Запрос на добавление материала в конкретную группу
        /// </summary>
        /// <param name="groupLearn"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateGroupLearnAsync([FromBody] GroupLearn groupLearn)
        {
            try
            {
                await _repo.AddAsync(groupLearn);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление материала из конкретной группы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveGroupLearnAsync([FromRoute] int id, [FromRoute] string timestamp)
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
