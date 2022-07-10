using AutoMapper;
using LearnApp.BL.Services;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/sharenote")]
    [ApiController]
    public class ShareNoteController : ControllerBase
    {
        private readonly IMapper _mapperNote;
        private readonly IMapper _mapperUser;
        private readonly ShareNoteService _service;

        public ShareNoteController(ShareNoteService service)
        {
            _service = service;

            //Игнорирование поля ShareLearn в объекте Note
            var learnConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<Note, Note>()
                .ForMember(x => x.ShareNotes, opt => opt.Ignore()));
            _mapperNote = learnConfig.CreateMapper();

            //Игнорирование поля ShareLearn в объекте User
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
        [HttpGet("user/{userGuid}")]
        public async Task<IEnumerable<Note>> GetNotesAsync(Guid userGuid) =>
            _mapperNote.Map<List<Note>, List<Note>>(await _service.GetShareAccessNotes(userGuid));

        /// <summary>
        /// Запрос на получение пользователей,
        /// кто имеет доступ к конкретной заметке
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <returns></returns>
        [HttpGet("note/{noteGuid}")]
        public async Task<IEnumerable<User>> GetUsersAsync(Guid noteGuid) =>
             _mapperUser.Map<List<User>, List<User>>(await _service.GetShareNoteUsersAsync(noteGuid));

        /// <summary>
        /// Запрос на создание открытого доступа к материалу
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpPost("{noteGuid}/{userGuid}")]
        public async Task<IActionResult> CreateShareAsync(Guid noteGuid, Guid userGuid)
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

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление открытого доступа к материалу
        /// </summary>
        /// <param name="noteGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpDelete("{noteGuid}/{userGuid}")]
        public async Task<IActionResult> RemoveShareAsync(Guid noteGuid, Guid userGuid)
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

            return Ok();
        }
    }
}
