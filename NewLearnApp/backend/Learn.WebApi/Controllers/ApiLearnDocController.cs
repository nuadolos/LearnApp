using AutoMapper;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiLearnDocController : ControllerBase
    {
        // Статья о работе с файлами в ASP.NET Core
        // https://docs.microsoft.com/ru-ru/aspnet/core/mvc/models/file-uploads?view=aspnetcore-6.0

        // Передача файлов на строну клиента
        // https://www.interestprograms.ru/source-codes-asp-net-download-files

        private readonly IMapper _mapper;
        private readonly ILearnDocumentsRepo _repo;

        public ApiLearnDocController(ILearnDocumentsRepo repo)
        {
            _repo = repo;

            //Игнорирование поля Learn в объекте LearnDocuments
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<LearnDocuments, LearnDocuments>()
                .ForMember(x => x.FileContent, opt => opt.Ignore())
                .ForMember(x => x.Learn, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех документов конкретного задания
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Learn/{id}")]
        public async Task<ActionResult<List<LearnDocuments>>> GetDocumentsAsync([FromRoute] int id) =>
            _mapper.Map<List<LearnDocuments>, List<LearnDocuments>>(await _repo.GetDocumentsAsync(id));

        /// <summary>
        /// Запрос на получение конкретного документа
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<LearnDocuments>> GetDocumentAsync([FromRoute] int id)
        {
            var doc = await _repo.GetRecordAsync(id);

            if (doc == null)
                return BadRequest(new ValidateError("Искомый документ отсутствует"));

            return Ok(doc);
        }

        /// <summary>
        /// Запрос на прикрепление документа к заданию
        /// </summary>
        /// <param name="learnDoc"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDocumentAsync([FromBody] LearnDocuments learnDoc)
        {
            string result = await _repo.LoadAsync(learnDoc);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление документа
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFriendAsync([FromRoute] int id)
        {
            var doc = await _repo.GetRecordAsync(id);

            if (doc == null)
                return BadRequest(new ValidateError("Искомый документ отсутствует"));

            try
            {
                await _repo.DeleteAsync(doc);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }
    }
}
