using LearnApp.DAL.Entities;
using System.Security.Cryptography;
using System.Text;

namespace LearnApp.Helper.Services
{
    /// <summary>
    /// Отвечает за сохранность и целостной пароля 
    /// </summary>
    static public class SecurityService
    {
        /// <summary>
        /// Хеширует пароль и возвращает используюмую соль
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string PasswordHashing(string password, out string salt)
        {
            salt = GenerateSalt();

            return SaltAndHashPassword(password, salt);
        }

        /// <summary>
        /// Проверяет пароль на соотвествие с хешируемым паролем
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordСonfirmation"></param>
        /// <returns></returns>
        public static bool CheckPassword(User user, string passwordСonfirmation) =>
            user.PasswordHash == SaltAndHashPassword(passwordСonfirmation, user.Salt);


        /// <summary>
        /// Генерирует соль, избавляющая от коллизий
        /// </summary>
        /// <returns></returns>
        private static string GenerateSalt()
        {
            var saltBytes = new byte[16];
            RandomNumberGenerator.Create().GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Шифрует солевой пароль с помощью алгоритма SHA256
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static string SaltAndHashPassword(string password, string salt) =>
            Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(password + salt)));
    }
}
