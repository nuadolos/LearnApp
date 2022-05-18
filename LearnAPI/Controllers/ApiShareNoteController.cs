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
        public async Task<IEnumerable<User>> GetUsersAsync([FromRoute] int id) =>
            _mapperUser.Map<List<User>, List<User>>(await _repo.GetUsersAsync(id));

        /// <summary>
        /// Запрос на создание открытого доступа к материалу
        /// </summary>
        /// <param name="shapeLearn"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateShareAsync([FromBody] ShareNote shareNote)
        {
            try
            {
                await _repo.AddAsync(shareNote);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление открытого доступа к материалу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> RemoveShareAsync([FromRoute] int id, [FromRoute] string timestamp)
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
