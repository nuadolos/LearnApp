using AutoMapper;
using LearnApp.BLL.Models;
using LearnApp.BLL.Models.Create;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly NoteService _service;

        public NoteController(NoteService service)
        {
            _service = service;

            //Игнорирование ссылочных полей объекта Note
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Note, Note>()
                .ForMember(x => x.NoteType, opt => opt.Ignore())
                .ForMember(x => x.ShareNotes, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех заметок конкретного пользователя
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <returns></returns>
        [HttpGet("{noteGuid}")]
        public async Task<IEnumerable<Note>> GetUserLearnsAsync(Guid noteGuid) =>
            _mapper.Map<List<Note>, List<Note>>(await _service.GetUserNotesAsync(noteGuid));

        /// <summary>
        /// Запрос на добавление новой заметки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSourceAsync(RequestNoteModel model)
        {
            try
            {
                return Ok(_mapper.Map<Note, Note>(await _service.CreateNoteAsync(model)));
            }
            catch (Exception ex)
            {
                // add logger test?
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Запрос на изменение заметки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{noteGuid}")]
        public async Task<IActionResult> UpdateLearnAsync(Guid noteGuid, RequestNoteModel model)
        {
            try
            {
                await _service.UpdateNoteAsync(noteGuid, model);
            }
            catch (Exception ex)
            {
                // add logger test?
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Запрос на удаление заметки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveSourceAsync(RequestRemoveDataModel model)
        {
            try
            {
                await _service.RemoveNoteAsync(model);
            }
            catch (Exception ex)
            {
                // add logger test?
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
