using AutoMapper;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Entities.WebModel;
using LearnApp.DAL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiShareNoteController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapperNote;
        private readonly IMapper _mapperUser;
        private readonly IShareNoteRepo _repo;

        public ApiShareNoteController(IShareNoteRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля ShareLearn в объекте Note
            var learnConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<Note, Note>()
                .ForMember(x => x.ShareNote, opt => opt.Ignore()));
            _mapperNote = learnConfig.CreateMapper();

            //Игнорирование поля ShareLearn в объекте User
            var userConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<User, User>()
                .ForMember(x => x.ShareNote, opt => opt.Ignore()));
            _mapperUser = userConfig.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение чужих заметок пользователем,
        /// имеющий к ним доступ
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("User/{email}")]
        public async Task<IEnumerable<Note>> GetNotesAsync([FromRoute] string email)
        {
            User user = await _userManager.FindByNameAsync(email);
            var notes = await _repo.GetNotesAsync(user.Id);
            return _mapperNote.Map<List<Note>, List<Note>>(notes);
        }

        /// <summary>
        /// Запрос на получение пользователей,
        /// кто имеет доступ к чужой заметке
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Note/{id}")]
        public IEnumerable<User> GetUsersAsync([FromRoute] int id) =>
            _mapperUser.Map<List<User>, List<User>>(_repo.GetUsersAsync(id));

        /// <summary>
        /// Запрос на создание открытого доступа к материалу
        /// </summary>
        /// <param name="shapeLearn"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateShareAsync([FromBody] OpenAccessNote shareNote)
        {
            User user = await _userManager.FindByEmailAsync(shareNote.Email);

            if (user == null)
                return BadRequest(new ValidateError("Пользователь не найден"));

            string result = await _repo.OpenAccessAsync(shareNote.NoteId, user.Id, shareNote.CanChange);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление открытого доступа к материалу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{noteId}/{userId}")]
        public async Task<IActionResult> RemoveShareAsync([FromRoute] int noteId, [FromRoute] string userId)
        {
            string result = await _repo.BlockAccessAsync(noteId, userId);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }
    }
}
