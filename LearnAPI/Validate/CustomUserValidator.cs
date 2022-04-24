using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace LearnAPI.Validate
{
    /// <summary>
    /// Класс проверки данных пользователя, реализующий интерфейс IUserValidator<User>
    /// </summary>
    public class CustomUserValidator : IUserValidator<User>
    {
        /// <summary>
        /// Метод ValidateAsync, вызывающийся при валидации пользователя
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            //Проверяет формат введенной почты
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (!Regex.IsMatch(user.Email, pattern))
            {
                errors.Add(new IdentityError { Description = $"Неправильный формат электронной почты" });
            }

            //Проверяет на наличие зарегистрированной почты
            foreach (var regUser in manager.Users)
            {
                if (user.Email.Contains(regUser.Email))
                {
                    errors.Add(new IdentityError { Description = $"Данная почта {user.Email} уже зарегистрирована" });
                    break;
                }
            }

            //Возвращает результат в зависимости от кол-ва найденных ошибок
            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
