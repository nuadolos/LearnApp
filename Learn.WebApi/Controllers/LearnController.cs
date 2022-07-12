using AutoMapper;
using LearnApp.BLL.Models;
using LearnApp.BLL.Services;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly LearnService _service;

        public LearnController(LearnService service)
        {
            _service = service;

            //Игнорирование ссылочных полей объекта Learn
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Learn, Learn>()
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.Group, opt => opt.Ignore())
                .ForMember(x => x.LearnDocs, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех заданий конкретной группы
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <returns></returns>
        [HttpGet("{groupGuid}")]
        public async Task<IEnumerable<Learn>> GetGroupLearnsAsync(Guid groupGuid) =>
            _mapper.Map<List<Learn>, List<Learn>>(await _service.GetGroupLearnsAsync(groupGuid));

        /// <summary>
        /// Запрос на получение всех заданий, созданные конкретным пользователем
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet("{userGuid}")]
        public async Task<IEnumerable<Learn>> GetCreatorLearnsAsync(Guid userGuid) =>
            _mapper.Map<List<Learn>, List<Learn>>(await _service.GetCreatorLearnsAsync(userGuid));

        /// <summary>
        /// Запрос на получение конкретного задания конкретным пользователем
        /// </summary>
        /// <param name="learnGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet("{learnGuid}/{userGuid}")]
        public async Task<ActionResult<Learn>> GetGroupLearnAsync(Guid learnGuid, Guid userGuid)
        {
            try
            {
                var learn = await _service.GetGroupLearnAsync(learnGuid, userGuid);
                return Ok(_mapper.Map<Learn, Learn>(learn));
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Запрос на добавление нового задания конкретным пользователем
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateLearnAsync(RequestLearnModel model)
        {
            try
            {
                var learn = await _service.CreateLearnAsync(model);
                return Ok(learn);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Запрос на изменение данных о задании
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{userGuid}")]
        public async Task<IActionResult> UpdateLearnAsync(Guid userGuid, RequestLearnModel model)
        {
            try
            {
                await _service.UpdateLearnAsync(userGuid, model);
            }
            catch (Exception ex)
            {
                // logger
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Запрос на удаление задания
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveLearnAsync(RequestRemoveDataModel model)
        {
            try
            {
                await _service.DeleteLearnAsync(model);
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
