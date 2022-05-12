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
        public IEnumerable<Learn> GetGroupLearns([FromRoute] int id) =>
            _mapper.Map<List<Learn>, List<Learn>>(_repo.GetGroupLearns(id));

        /// <summary>
        /// Запрос на получение конкретной записи материала группы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<GroupLearn> GetGroupLearn([FromRoute] int id)
        {
            var groupLearn = _repo.GetRecord(id);

            if (groupLearn != null)
                return Ok(groupLearn);

            return NotFound(new List<ValidateError> { 
                new ValidateError("Нет данных о материале, принадлежащий конкретной группе") 
            });
        }

        /// <summary>
        /// Запрос на добавление материала в конкретную группу
        /// </summary>
        /// <param name="groupLearn"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateGroupLearn([FromBody] GroupLearn groupLearn)
        {
            try
            {
                _repo.Add(groupLearn);
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
        /// Запрос на удаление материала из конкретной группы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public IActionResult RemoveGroupLearn([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
                timestamp = $"\"{timestamp}\"";

            if (timestamp.Contains("%2F"))
                timestamp = timestamp.Replace("%2F", "/");

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
