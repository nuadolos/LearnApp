using AutoMapper;
using LearnEF.Entities;
using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using LearnEF.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiNoteController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly INoteRepo _repo;

        public ApiNoteController(INoteRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

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
        [HttpGet("User/{email}")]
        public async Task<IEnumerable<Note>> GetUserLearnsAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var notes = await _repo.GetUserNotes(user.Id);
            return _mapper.Map<List<Note>, List<Note>>(notes);
        }

        /// <summary>
        /// Запрос на получение всех ресурсов
        /// </summary>
        /// <returns></returns>
        [HttpGet("sources")]
        public async Task<IEnumerable<SourceLore>> GetSourcesAsync() =>
            _mapper.Map<List<SourceLore>, List<SourceLore>>(await _repo.GetSourceLoresAsync());

        /// <summary>
        /// Запрос на получение конкретной заметки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{email}/{id}/{act}")]
        public async Task<ActionResult<Note>> GetNoteAsync([FromRoute] string email, [FromRoute] int id, [FromRoute] string act)
        {
            var note = await _repo.GetRecordAsync(id);

            if (note == null)
                return NotFound(new ValidateError("Заметка не найдена"));

            User user = await _userManager.FindByEmailAsync(email);

            // Определяет, какое действие было вызвано
            switch (act)
            {
                case "Details":
                    {
                        // Проверяет, чужой ли пользователь пытается запросить данные
                        if (note.UserId != user.Id && !await _repo.SharedWithAsync(note.Id, user.Id))
                            return BadRequest(new ValidateError("Вы не имеете доступ к данной заметке"));

                        break;
                    }
                case "Edit":
                    {
                        // Проверяет, запрашивает ли автор заметки
                        if (note.UserId != user.Id)
                        {
                            // Проверяет, имеет ли доступ к редактированию
                            // данной заметки чужой пользователь
                            if (!await _repo.CanChangeLearnAsync(note.Id, user.Id))
                                return BadRequest(new ValidateError("Вы не имеете доступ к редактированию данной заметки"));
                        }

                        break;
                    }
                case "Delete":
                    {
                        // Проверяет, запрашивает ли автор заметку
                        if (note.UserId != user.Id)
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
        [HttpPost("{email}")]
        public async Task<IActionResult> CreateSourceAsync([FromRoute] string email, [FromBody] Note note)
        {
            User user = await _userManager.FindByEmailAsync(email);

            note.CreateDate = DateTime.Now;
            note.UserId = user.Id;

            try
            {
                await _repo.AddAsync(note);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на изменение заметки
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLearnAsync([FromBody] Note note)
        {
            try
            {
                await _repo.UpdateAsync(note);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление заметки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveSourceAsync([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
                timestamp = $"\"{timestamp}\"";

            if (timestamp.Contains("%2F"))
                timestamp = timestamp.Replace("%2F", "/");

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);

            string result = await _repo.DeleteLearnAsync(id, ts);

            return result == string.Empty ? Ok() : BadRequest(new ValidateError(result));
        }
    }
}
