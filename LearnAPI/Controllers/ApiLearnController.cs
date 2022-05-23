using AutoMapper;
using LearnEF.Entities;
using LearnEF.Entities.Base;
using LearnEF.Entities.ErrorModel;
using LearnEF.Entities.IdentityModel;
using LearnEF.Entities.WebModel;
using LearnEF.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiLearnController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILearnRepo _repo;

        public ApiLearnController(ILearnRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование ссылочных полей объекта Learn
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Learn, Learn>()
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.Group, opt => opt.Ignore())
                .ForMember(x => x.LearnDocuments, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех материалов конкретной группы
        /// </summary>
        /// <returns></returns>
        [HttpGet("Group/{groupId}")]
        public async Task<IEnumerable<Learn>> GetGroupLearnsAsync([FromRoute] int groupId) =>
            _mapper.Map<List<Learn>, List<Learn>>(await _repo.GetGroupLearnsAsync(groupId));

        /// <summary>
        /// Запрос на получение конкретного материала
        /// конкретной группы с учетом
        /// вызванного действия и залогиненного пользователя
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="email"></param>
        /// <param name="learnId"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        [HttpGet("{groupId}/{email}/{learnId}/{act}")]
        public async Task<ActionResult<Learn>> GetGroupLearnAsync([FromRoute] int groupId, [FromRoute] string email,
            [FromRoute] int learnId, [FromRoute] string act)
        {
            var group = await _repo.GetGroupAsync(groupId);
            var learn = await _repo.GetLearnAsync(learnId);

            if (group == null)
                return NotFound(new ValidateError("Искомая группа не существует"));

            if (learn == null)
                return NotFound(new ValidateError("Искомый материал отсутствует"));

            if (learn.GroupId != group.Id)
                return NotFound(new ValidateError("Данный материал отсутствует в группе"));

            User user = await _userManager.FindByNameAsync(email);

            // Определяет, какое действие было вызвано
            switch (act)
            {
                case "Details":
                    {
                        // Проверяет, участник ли группы пытается запросить данные
                        if (group.UserId != user.Id && !await _repo.IsMemberGroupAsync(groupId, user.Id))
                            return BadRequest(new ValidateError("Вы не имеете доступ к данному материалу"));

                        break;
                    }
                case "Edit":
                    {
                        // Проверяет, запрашивает ли создатель группы
                        if (group.UserId != user.Id)
                        {
                            // Проверяет, имеет ли доступ к редактированию
                            // данной записи участник группы
                            if (!await _repo.CanChangeLearnAsync(groupId, user.Id))
                                return BadRequest(new ValidateError("Вы не имеете доступ к редактированию данного материала"));
                        }

                        break;
                    }
                case "Delete":
                    {
                        // Проверяет, запрашивает ли автор запись
                        if (group.UserId != user.Id && learn.UserId != user.Id)
                            return BadRequest(new ValidateError("Доступ к удалению данного материала имеет только автор или создатель группы"));

                        break;
                    }
                default:
                    return BadRequest(new ValidateError("Запрос не имеет действия"));
            }

            return Ok(_mapper.Map<Learn, Learn>(learn));
        }

        /// <summary>
        /// Запрос на добавление нового материала
        /// и его привязка к конкретному пользователю
        /// </summary>
        /// <param name="email"></param>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPost("{email}")]
        public async Task<IActionResult> CreateLearnAsync([FromRoute] string email, [FromBody] FullLearn fullLearn)
        {
            User user = await _userManager.FindByNameAsync(email);

            List<LearnDocuments>? documents = null;
            Learn learn = new Learn {
                Title = fullLearn.Title,
                Description = fullLearn.Description,
                CreateDate = fullLearn.CreateDate,
                Deadline = fullLearn.Deadline,
                GroupId = fullLearn.GroupId,
                UserId = user.Id
            };

            if (fullLearn.Files != null)
            {
                documents = new List<LearnDocuments>();

                foreach (var doc in fullLearn.Files)
                {
                    documents.Add(new LearnDocuments { Name = doc.Name, FileContent = doc.FileContent });
                }
            }

            string result = await _repo.CreateFullLearnAsync(learn, documents);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }

        /// <summary>
        /// Запрос на изменение материала
        /// </summary>
        /// <param name="learn"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLearnAsync([FromBody] Learn learn)
        {
            try
            {
                await _repo.UpdateAsync(learn);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление материала
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveLearnAsync([FromRoute] int id)
        {
            string result = await _repo.DeleteAllDataLearnAsync(id);

            if (result != string.Empty)
                return BadRequest(new ValidateError(result));

            return Ok();
        }
    }
}
