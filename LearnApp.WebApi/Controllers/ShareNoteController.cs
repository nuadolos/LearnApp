using AutoMapper;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShareNoteController : ControllerBase
    {
        private readonly IMapper _mapperNote;
        private readonly IMapper _mapperUser;
        private readonly ShareNoteService _service;

        public ShareNoteController(ShareNoteService service)
        {
            _service = service;

            //Игнорирование ссылочных полей в объекте Note
            var learnConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<Note, Note>()
                .ForMember(x => x.ShareNotes, opt => opt.Ignore()));
            _mapperNote = learnConfig.CreateMapper();

            //Игнорирование ссылочных полей в объекте User
            var userConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<User, User>()
                .ForMember(x => x.ShareNotes, opt => opt.Ignore()));
            _mapperUser = userConfig.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение чужих заметок пользователем,
        /// имеющий к ним доступ
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet("{userGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes(Guid userGuid) =>
            _mapperNote.Map<List<Note>, List<Note>>(await _service.GetShareAccessNotes(userGuid));

        /// <summary>
        /// Запрос на получение пользователей,
        /// кто имеет доступ к конкретной заметке
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <returns></returns>
        [HttpGet("{noteGuid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(Guid noteGuid) =>
             _mapperUser.Map<List<User>, List<User>>(await _service.GetShareNoteUsersAsync(noteGuid));

        /// <summary>
        /// Запрос на создание открытого доступа к материалу
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpPost("{noteGuid}/{userGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateShare(Guid noteGuid, Guid userGuid)
        {
            try
            {
                await _service.AddShareAccessAsync(noteGuid, userGuid);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Запрос на удаление открытого доступа к материалу
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpDelete("{noteGuid}/{userGuid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveShare(Guid noteGuid, Guid userGuid)
        {
            try
            {
                await _service.RemoveShareAccessAsync(noteGuid, userGuid);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
