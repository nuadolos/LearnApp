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
    public class FollowerService
    {
        private readonly IFollowerRepo _repo;
        
        public FollowerService(IFollowerRepo repo) =>
            _repo = repo;

        /// <summary>
        /// Возвращает список пользователей,
        /// на которых подписан конкретный пользователь
        /// </summary>
        /// <param name="subUserGuid"></param>
        /// <returns></returns>
        public async Task<List<User>> GetFollowingAsync(Guid subUserGuid) =>
            await _repo.GetFollowingAsync(subUserGuid);

        /// <summary>
        /// Возвращает список пользователей,
        /// которые подписаны на конкретного пользователя
        /// </summary>
        /// <param name="trackUserGuid"></param>
        /// <returns></returns>
        public async Task<List<User>> GetFollowersAsync(Guid trackUserGuid) =>
            await _repo.GetFollowersAsync(trackUserGuid);

        /// <summary>
        /// Оформляет подписку на конкретного пользователя
        /// </summary>
        /// <param name="subUserGuid"></param>
        /// <param name="trackUserGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task FollowAsync(Guid subUserGuid, Guid trackUserGuid)
        {
            var follower = new Follower {
                SubscribeUserGuid = subUserGuid,
                TrackedUserGuid = trackUserGuid
            };

            try
            {
                await _repo.AddAsync(follower);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При попытки подписаться на пользователя {trackUserGuid}" +
                    $" у пользователя {subUserGuid} возникла ошибка: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Отменяет подписку на конкретного пользователя
        /// </summary>
        /// <param name="subUserGuid"></param>
        /// <param name="trackUserGuid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UnfollowAsync(Guid subUserGuid, Guid trackUserGuid)
        {
            var follower = await _repo.GetFollowerAsync(subUserGuid, trackUserGuid);

            if (follower == null)
                throw new Exception($"Пользователь {subUserGuid} попытался отписать" +
                    $" от пользователя {trackUserGuid}, не имея на него подписку");

            try
            {
                await _repo.DeleteAsync(follower);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При попытки подписаться на пользователя {trackUserGuid}" +
                    $" у пользователя {subUserGuid} возникла ошибка: {ex.Message}", ex);
            }
        }
    }
}
