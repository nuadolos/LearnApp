using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnApp.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string? Role { get; set; } = null;
        public string? Policy { get; set; } = null;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"] as User;

            if (user is null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            if (Role is not null)
            {
                if (Role != user.UserRoleCode)
                    context.Result = new JsonResult(new { message = "Ресурс заблокирован" }) { StatusCode = StatusCodes.Status423Locked };
                return;
            }

            if (Policy is not null)
            {

            }
        }
    }
}
