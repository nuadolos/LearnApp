using AutoMapper;
using LearnApp.BLL.Models.Request;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Note>))]
        public async Task<ActionResult<IEnumerable<Note>>> GetUserNotes(Guid noteGuid) =>
            _mapper.Map<List<Note>, List<Note>>(await _service.GetUserNotesAsync(noteGuid));

        /// <summary>
        /// Запрос на добавление новой заметки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Note>> CreateNote(RequestNoteModel model)
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
        /// <param name="noteGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{noteGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateNote(Guid noteGuid, RequestNoteModel model)
        {
            try
            {
                string error = await _service.UpdateNoteAsync(noteGuid, model);

                if (!string.IsNullOrEmpty(error))
                {
                    // add log warning
                    return BadRequest(error);
                }
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveNote(RequestRemoveDataModel model)
        {
            try
            {
                string error = await _service.RemoveNoteAsync(model);

                if (!string.IsNullOrEmpty(error))
                {
                    // add log warning
                    return BadRequest(error);
                }
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
