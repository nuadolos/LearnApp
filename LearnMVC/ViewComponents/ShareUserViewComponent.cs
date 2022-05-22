using LearnEF.Entities.IdentityModel;
using LearnHTTP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace LearnMVC.ViewComponents
{
    public class ShareUserViewComponent : ViewComponent
    {
        private readonly string _shareUrl;

        public ShareUserViewComponent(IConfiguration configuration) =>
            _shareUrl = configuration.GetSection("ShareNoteAddress").Value;

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var users = await HttpRequestClient.GetRequestAsync<List<User>>(_shareUrl, "Note", id.ToString());

            return users != null ? View("ShareUserPartial", users) : new ContentViewComponentResult("Вы ни с кем не делились записью");
        }
    }
}
