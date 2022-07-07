using AutoMapper;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Entities.WebModel;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INoteRepo _repo;

        public NoteController(INoteRepo repo)
        {
            _repo = repo;

            //Игнорирование ссылочных полей объекта Note
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Note, Note>()
                .ForMember(x => x.SourceLore, opt => opt.Ignore())
                .ForMember(x => x.ShareNote, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех заметок конкретного пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<IEnumerable<Note>> GetUserLearnsAsync(string id) =>
            _mapper.Map<List<Note>, List<Note>>(await _repo.GetUserNotesAsync(id));

        /// <summary>
        /// Запрос на получение конкретной заметки
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        [HttpGet("{userId}/{id}/{act}")]
        public async Task<IActionResult> GetNoteAsync(string userId, int id, string act)
        {
            var note = await _repo.GetRecordAsync(id);

            if (note == null)
                return NotFound(new ValidateError("Заметка не найдена"));

            // Определяет, какое действие было вызвано
            switch (act)
            {
                case "Details":
                    {
                        // Проверяет, чужой ли пользователь пытается запросить данные
                        if (note.UserId != userId && !await _repo.SharedWithAsync(note.Id, userId))
                            return BadRequest(new ValidateError("Вы не имеете доступ к данной заметке"));

                        break;
                    }
                case "Edit":
                    {
                        // Проверяет, запрашивает ли автор заметки
                        if (note.UserId != userId)
                        {
                            // Проверяет, имеет ли доступ к редактированию
                            // данной заметки чужой пользователь
                            if (!await _repo.CanChangeNoteAsync(note.Id, userId))
                                return BadRequest(new ValidateError("Вы не имеете доступ к редактированию данной заметки"));
                        }

                        break;
                    }
                case "Delete":
                    {
                        // Проверяет, запрашивает ли автор заметку
                        if (note.UserId != userId)
                            return BadRequest(new ValidateError("Доступ к удалению данного материала имеет только автор или создатель группы"));

                        break;
                    }
                default:
                    return BadRequest(new ValidateError("Запрос не имеет действия"));
            }

            return Ok(_mapper.Map<Note, Note>(note));
        }

        /// <summary>
        /// Запрос на добавление новой заметки
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSourceAsync(Note note)
        {
            try
            {
                await _repo.AddAsync(note);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }
            
            return Ok(_mapper.Map<Note, Note>(note));
        }

        /// <summary>
        /// Запрос на изменение заметки
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateLearnAsync(Note note)
        {
            try
            {
                await _repo.UpdateAsync(note);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return NoContent();
        }

        /// <summary>
        /// Запрос на удаление заметки
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveSourceAsync(DeleteModel note)
        {
            string result = await _repo.DeleteNoteAsync(note.Id, note.Timestamp);

            return result == string.Empty ? NoContent() : BadRequest(new ValidateError(result));
        }
    }
}
