using LearnApp.DAL.Entities;
using LearnApp.DAL.Repos.IRepos;
using LearnApp.Helper.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnApp.WebApi.Services
{
    public class JwtService
    {
        private readonly IUserRepo _repo;
        private readonly IConfiguration _config;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IUserRepo repo, IConfiguration config, ILogger<JwtService> logger)
        {
            _repo = repo;
            _config = config;
            _logger = logger;
        }

        public string GenerateJwtToken(Guid userGuid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("guid", userGuid.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task VerifyAsync(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_config["Secret"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var userGuid = jwtToken.Claims.First(claim => claim.Type == "guid").Value;

                    context.Items["User"] = await _repo.GetByGuidAsync(Guid.Parse(userGuid));
                }
            }
            catch (Exception ex)
            {
                // todo: think about refreshing the access token
                _logger.LogErrorWithContext(ex, "Access token {token} is not validated", new object[] { token });   
            }
        }
    }
}
