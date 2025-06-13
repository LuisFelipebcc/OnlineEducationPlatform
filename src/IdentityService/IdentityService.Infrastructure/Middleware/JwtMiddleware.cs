using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace IdentityService.Infrastructure.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtMiddleware(RequestDelegate next, string secretKey, string issuer, string audience)
        {
            _next = next;
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "sub").Value);
                var roles = jwtToken.Claims.Where(x => x.Type == "role").Select(x => x.Value);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, jwtToken.Claims.First(x => x.Type == "name").Value)
                };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var identity = new ClaimsIdentity(claims);
                context.User = new ClaimsPrincipal(identity);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401;
                var response = new { message = "Invalid token" };
                var json = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(json).Wait();
            }
        }
    }
}