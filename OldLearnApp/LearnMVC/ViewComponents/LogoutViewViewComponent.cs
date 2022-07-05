using Microsoft.AspNetCore.Mvc;

namespace LearnMVC.ViewComponents
{
    public class LogoutViewViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
