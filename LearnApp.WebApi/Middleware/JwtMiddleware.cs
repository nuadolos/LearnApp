using LearnApp.DAL.Repos.IRepos;
using LearnApp.Helper.Services;
using LearnApp.WebApi.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LearnApp.WebApi.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, JwtService service)
        {
            var token = context.Request.Cookies["access_token"];

            if (token is not null)
                await service.VerifyAsync(context, token);

            await _next(context);
        }
    }
}
