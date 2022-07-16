using LearnApp.BLL.Models.Request;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Exceptions;
using LearnApp.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BLL.Services
{
    public class LearnService
    {
        private readonly ILearnRepo _repo;
        public LearnService(ILearnRepo repo) =>
            _repo = repo;

        /// <summary>
        /// Возвращает список заданий,
        /// расположенные в конкретной группе
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <returns></returns>
        public async Task<List<Learn>> GetGroupLearnsAsync(Guid groupGuid) =>
            await _repo.GetGroupLearnsAsync(groupGuid);

        /// <summary>
        /// Возвращает список заданий, 
        /// созданные конкретным пользователем
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public async Task<List<Learn>> GetCreatorLearnsAsync(Guid userGuid) =>
            await _repo.GetCreatorLearnsAsync(userGuid); // todo: на карточке задания показать, к какой группе он относится 

        /// <summary>
        /// Возвращает конкретное задание пользователю, 
        /// который имеет к ней доступ 
        /// </summary>
        /// <param name="learnGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Learn> GetGroupLearnAsync(Guid learnGuid, Guid userGuid)
        {
            // todo: проверить, какой метод будет оптимизирован лучше
            //       GetLearnByGuidAsync - LearnRepo или GetGroupByGuidAsync - GroupRepo
            var learn = await _repo.GetLearnByGuidAsync(learnGuid);

            if (learn == null)
                throw new Exception($"Задания {learnGuid} не существует");

            if (learn.UserGuid != userGuid && !learn.Group.GroupUsers.Any(l => l.UserGuid == userGuid))
                throw new Exception($"Пользователь {userGuid} пытается получить данные задания, не имея к нему доступ");

            return learn;
        }

        /// <summary>
        /// Создает задание конкретного пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Learn> CreateLearnAsync(RequestLearnModel model)
        {
            var learn = new Learn {
                Title = model.Title,
                Description = model.Description,
                Deadline = model.Deadline,
                GroupGuid = model.GroupGuid,
                UserGuid = model.UserGuid
            };

            try
            {
                await _repo.AddAsync(learn);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При добавлении задания у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex);
            }

            return learn;
        }

        /// <summary>
        /// Обновляет свойства задания, которые изменил пользователь
        /// </summary>
        /// <param name="learnGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateLearnAsync(Guid learnGuid, RequestLearnModel model)
        {
            var learn = await _repo.GetLearnByGuidAsync(learnGuid);

            if (learn == null)
                throw new Exception($"Задания {learnGuid} не существует");

            if (learn.UserGuid != model.UserGuid && 
                !learn.Group.GroupUsers.Any(l => l.UserGuid == model.UserGuid))
                throw new Exception($"Пользователь {model.UserGuid} пытается изменить данные задания, не имея к нему доступ");

            learn.Title = model.Title;
            learn.Description = model.Description;
            learn.Deadline = model.Deadline;

            try
            {
                await _repo.UpdateAsync(learn);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При обновлении задания у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Удаляет задание пользователем, создавший ее
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteLearnAsync(RequestRemoveDataModel model)
        {
            var learn = await _repo.GetLearnByGuidAsync(model.Guid);

            if (learn == null)
                throw new Exception($"Задания {model.Guid} не существует");

            if (learn.UserGuid != model.UserGuid &&
                !learn.Group.GroupUsers.Any(l => l.UserGuid == model.UserGuid))
                throw new Exception($"Пользователь {model.UserGuid} пытается удалить данные задания, не имея к нему доступ");

            try
            {
                await _repo.DeleteAsync(learn);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При удалении задания у пользователя {model.UserGuid} возникла ошибка: {ex.Message}", ex);
            }
        }
    }
}
