﻿using AutoMapper;
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
    public class ApiGroupController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IGroupRepo _repo;

        public ApiGroupController(IGroupRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

            //Игнорирование поля GroupType, GroupUser и GroupLearn в объекте Group
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<Group, Group>()
                .ForMember(x => x.GroupType, opt => opt.Ignore())
                .ForMember(x => x.GroupUser, opt => opt.Ignore())
                .ForMember(x => x.GroupLearn, opt => opt.Ignore()));

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Запрос на получение всех групп
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Group> GetGroups()
        {
            var groups = _repo.GetAll();
            return _mapper.Map<List<Group>, List<Group>>(groups);
        }

        /// <summary>
        /// Запрос на получение конкретной группы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Group> GetGroup([FromRoute] int id)
        {
            var group = _repo.GetRecord(id);

            if (group == null)
                return NotFound(new List<ValidateError> { new ValidateError("Группа не найдена") });

            return Ok(_mapper.Map<Group, Group>(group));
        }

        /// <summary>
        /// Запрос на создание группы
        /// </summary>
        /// <param name="email"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost("{email}")]
        public async Task<IActionResult> CreateGroup([FromRoute] string email, [FromBody] Group group)
        {
            try
            {
                User user = await _userManager.FindByNameAsync(email);
                group.UserId = user.Id;
                group.CreateDate = DateTime.Now;
                _repo.Add(group);
            }
            catch (DbMessageException ex)
            {
                //Получение ошибок при создании записи
                List<ValidateError> errors = new List<ValidateError>();

                errors.Add(new ValidateError(ex.Message));

                return BadRequest(errors);
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на изменение группы
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateGroup([FromBody] Group group)
        {
            try
            {
                _repo.Update(group);
            }
            catch (DbMessageException ex)
            {
                //Получение ошибок при создании записи
                List<ValidateError> errors = new List<ValidateError>();

                errors.Add(new ValidateError(ex.Message));

                return BadRequest(errors);
            }

            return Ok();
        }

        /// <summary>
        /// Запрос на удаление группы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public IActionResult RemoveGroup([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
                timestamp = $"\"{timestamp}\"";

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);

            try
            {
                _repo.Delete(id, ts);
            }
            catch (DbMessageException ex)
            {
                //Получение ошибок при создании записи
                List<ValidateError> errors = new List<ValidateError>();

                errors.Add(new ValidateError(ex.Message));

                return BadRequest(errors);
            }

            return Ok();
        }
    }
}
