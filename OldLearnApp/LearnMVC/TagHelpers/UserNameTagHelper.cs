using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LearnMVC.TagHelpers
{
    /// <summary>
    /// Дескриптор для указания отображение роли и имени пользователя
    /// </summary>
    public class UserNameTagHelper : TagHelper
    {
        private readonly UserManager<User> _userManager;

        public UserNameTagHelper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Отображается в дескрипторе как name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Отображается в дескрипторе как role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Используется при обращении к функции дескриптора user-name
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            User u = await _userManager.FindByNameAsync(Name);

            output.Content.SetContent($"{Role}, {u?.Name}");
        }
    }
}
