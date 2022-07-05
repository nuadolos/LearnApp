using LearnApp.DAL.Repos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LearnApp.WebApi.Helper
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IUserRepo repo)
        {
            var token = context.Request.Cookies["token"];

            if (token != null)
                await _configuration.VerifyAsync(context, repo, token);

            await _next(context);
        }
    }
}
