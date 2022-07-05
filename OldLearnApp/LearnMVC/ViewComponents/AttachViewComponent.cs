using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using LearnHTTP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace LearnMVC.ViewComponents
{
    public class AttachViewComponent : ViewComponent
    {
        private readonly string _attachUrl;

        public AttachViewComponent(IConfiguration configuration) =>
            _attachUrl = configuration.GetSection("AttachAddress").Value;

        public async Task<IViewComponentResult> InvokeAsync(Learn learn)
        {
            string? userName = User?.Identity?.Name;

            if (userName == null)
                return new ContentViewComponentResult("Вы не авторизованы");

            if (learn.UserRoleName == "Студент")
            {
                var attach = await HttpRequestClient.GetRequestAsync<Attach>(_attachUrl, "Get", userName, learn.Id.ToString());

                return View("StudentPartial", attach ?? new Attach { LearnId = learn.Id, IsAttached = false });
            }
            else
            {
                var attaches = await HttpRequestClient.GetRequestAsync<List<Attach>>(_attachUrl, learn.Id.ToString());

                return attaches?.Count > 0 ? View("AllPartial", attaches) : new ContentViewComponentResult("Никто не сдал задание");
            }
        }
    }
}
