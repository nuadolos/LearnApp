using AutoMapper;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.ErrorModel;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAttachController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAttachRepo _repo;

        public ApiAttachController(IAttachRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля Learn и User в объекте Attach
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Attach, Attach>()
                .ForMember(x => x.Learn, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех файлов студентов, принадлежащих конкретному материалу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{learnId}")]
        public async Task<IEnumerable<Attach>> GetAttachesAsync([FromRoute] int learnId)
        {
            var attaches = await _repo.GetLearnAttachesAsync(learnId);

            foreach (var attach in attaches)
            {
                attach.UserName = $"{attach.User.Surname} {attach.User.Name}";
            }

            return _mapper.Map<List<Attach>, List<Attach>>(attaches);
        }

        /// <summary>
        /// Запрос на получение конкретной прикрепленной записи студента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<LearnDoc>> GetAttachAsync([FromRoute] int id)
        {
            var attach = await _repo.GetRecordAsync(id);

            if (attach == null)
                return BadRequest(new ValidateError("Прикрепленный ответ к заданию отсутствует"));

            return Ok(_mapper.Map<Attach, Attach>(attach));
        }

        /// <summary>
        /// Запрос на получение конкретной прикрепленной записи студента
        /// с помощью учетной записи,
        /// через которую выложено сделанное задание
        /// </summary>
        /// <param name="email"></param>
        /// <param name="learnId"></param>
        /// <returns></returns>
        [HttpGet("Get/{email}/{learnId}")]
        public async Task<ActionResult<LearnDoc>> GetAttachAsync([FromRoute] string email, [FromRoute] int learnId)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound(new ValidateError("Пользователь не найден"));

            var attach = await _repo.GetAttachAsync(learnId, user.Id);

            if (attach == null)
                return BadRequest(new ValidateError("Прикрепленный ответ к заданию отсутствует"));

            return Ok(_mapper.Map<Attach, Attach>(attach));
        }

        /// <summary>
        /// Запрос на прикрепление сделанного задания студента
        /// </summary>
        /// <param name="email"></param>
        /// <param name="attach"></param>
        /// <returns></returns>
        [HttpPost("{email}")]
        public async Task<IActionResult> CreateAttachAsync([FromRoute] string email, [FromBody] Attach attach)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound(new ValidateError("Пользователь не найден"));

            attach.UserId = user.Id;

            try
            {
                await _repo.AddAsync(attach);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на изменение оценки сделанного задания студента
        /// </summary>
        /// <param name="email"></param>
        /// <param name="learnId"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPut("{id}/{rating}")]
        public async Task<IActionResult> UpdateAttachAsync([FromRoute] int id, [FromRoute] int rating)
        {
            var attach = await _repo.GetRecordAsync(id);

            if (attach == null)
                return NotFound(new ValidateError("Прикрепленное сделанное задания отсутствует"));

            attach.Rating = rating;

            try
            {
                await _repo.UpdateAsync(attach);
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на открепление файла сделанного задания
        /// или удаление этой записи студента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAttachAsync([FromRoute] int id)
        {
            var attach = await _repo.GetRecordAsync(id);

            if (attach == null)
                return NotFound(new ValidateError("Прикрепленное сделанное задания отсутствует"));

            try
            {
                if (attach.Rating == 0)
                {
                    await _repo.DeleteAsync(attach);
                }
                else
                {
                    attach.FileContent = null;
                    await _repo.UpdateAsync(attach);
                }
            }
            catch (DbMessageException ex)
            {
                return BadRequest(new ValidateError(ex.Message));
            }

            return Ok();
        }
    }
}
