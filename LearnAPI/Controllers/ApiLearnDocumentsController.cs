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
    public class ApiLearnDocumentsController : ControllerBase
    {
        // Статья о работе с файлами в ASP.NET Core
        // https://docs.microsoft.com/ru-ru/aspnet/core/mvc/models/file-uploads?view=aspnetcore-6.0

        // Передача файлов на строну клиента
        // https://www.interestprograms.ru/source-codes-asp-net-download-files

        private readonly IMapper _mapper;
        private readonly ILearnDocumentsRepo _repo;

        public ApiLearnDocumentsController(ILearnDocumentsRepo repo)
        {
            _repo = repo;

            //Игнорирование поля SentUser и AcceptedUser в объекте Friend
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<LearnDocuments, LearnDocuments>()
                .ForMember(x => x.Learn, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех документов конкретного материала
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("Learn/{id}")]
        public async Task<IEnumerable<LearnDocuments>> GetDocumentsAsync([FromRoute] int id) =>
            _mapper.Map<List<LearnDocuments>, List<LearnDocuments>>(await _repo.GetDocumentsAsync(id));

        /// <summary>
        /// Запрос на получение конкретного документа
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Friend>> GetDocumentAsync([FromRoute] int id)
        {
            var learnDoc = await _repo.GetRecordAsync(id);

            if (learnDoc != null)
                return Ok(learnDoc);

            return NotFound(new List<ValidateError> { new ValidateError("Нет данных о документе") });
        }

        /// <summary>
        /// Запрос на прикрепление документа к материалу
        /// </summary>
        /// <param name="learnDoc"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDocumentAsync([FromBody] LearnDocuments learnDoc)
        {
            try
            {
                await _repo.AddAsync(learnDoc);
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
        /// Запрос на удаление документа
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveFriendAsync([FromRoute] int id, [FromRoute] string timestamp)
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
