using LearnApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnApp.WebApi.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string? Role { get; set; } = null;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"] as User;

            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) {
                    StatusCode = StatusCodes.Status401Unauthorized 
                };

                return;
            }

            //if (Role != null)
            //{
            //    if (Role != user.Name)
            //        context.Result = new JsonResult(new { message = "Вы не Владимир..." }) { StatusCode = StatusCodes.Status423Locked };

            //    return;
            //}
        }
    }
}
