using LearnApp.BLL.Models.Request;
using LearnApp.DAL.Entities;
using LearnApp.DAL.Exceptions;
using LearnApp.DAL.Repos.IRepos;
using LearnApp.Helper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BLL.Services
{
    public class AccountService
    {
        private readonly IUserRepo _repo;

        public AccountService(IUserRepo repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Регистрирует нового пользователя в системе
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RegisterAsync(RequestRegisterModel model)
        {
            User user = new User
            {
                Login = model.Login,
                Surname = model.Surname,
                Name = model.Name,
                UserRoleCode = "COMMON"
            };

            user.PasswordHash = SecurityService.PasswordHashing(model.Password, out string salt);
            user.Salt = salt;

            try
            {
                await _repo.AddAsync(user);
            }
            catch (DbMessageException ex)
            {
                throw new Exception($"При сохранении нового пользователя возникла ошибка: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Производит аутентификацию пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(User? user, string error)> LoginAsync(RequestLoginModel model)
        {
            var user = await _repo.GetByLoginAsync(model.Login);

            if (user == null)
                return (user: null, error: "Пользователь не найден");

            if (!SecurityService.CheckPassword(user, model.Password))
                return (user: null, error: "Логин или пароль указаны некорректно");

            return (user, error: string.Empty);
        }
    }
}
