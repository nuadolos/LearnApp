using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace LearnAPI.Validate
{
    /// <summary>
    /// Класс проверки пароля, реализующий интерфейс IPasswordValidator<User>
    /// </summary>
    public class CustomPasswordValidator : IPasswordValidator<User>
    {
        /// <summary>
        /// Минимальная длина пароля
        /// </summary>
        public int RequiredLength { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="length"></param>
        public CustomPasswordValidator(int length) => RequiredLength = length;

        /// <summary>
        /// Метод ValidateAsync, вызывающийся при валидации пароля
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (string.IsNullOrEmpty(password) || password.Length < RequiredLength)
            {
                errors.Add(new IdentityError { Description = $"Минимальная длина пароля {RequiredLength} символов" });
            }

            if (password.Length > 20)
            {
                errors.Add(new IdentityError { Description = $"Максимальная длина пароля 20 символов" });
            }

            string pattern = @"[A-z0-9]+$";

            //Проверяет на присутствие символов, не являющиеся буквой или цифрой
            if (!Regex.IsMatch(password, pattern))
            {
                errors.Add(new IdentityError { Description = "Пароль должен состоять из алфавитных или цифровых символов" });
            }

            //Возвращает результат в зависимости от кол-ва найденных ошибок
            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
