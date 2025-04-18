using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Application.Helpers;
public class AuthTokenHandler(IHttpContextAccessor accessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage? response = null;
            try
            {
                var token = "";
                var tokenMethod = "";

                if (accessor.HttpContext?.User.Identity?.IsAuthenticated is true)
                {
                    tokenMethod = accessor.HttpContext!.User.FindFirstValue(ClaimTypes.AuthenticationMethod) ??
                                  "Bearer";
                    token = accessor.HttpContext.Request.Cookies["token"].ToString();
                }

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue( tokenMethod,token);
                }

                response = await base.SendAsync(request, cancellationToken);
                return response;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                throw;
            }
        }
        public static string GenerateToken(ClaimsIdentity claims)
        {
            var handler = new JwtSecurityTokenHandler();

            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c5d4daef4df64b08b4ce630a38c0005e10a5953f519c2f1d143379784689fdd4")),
                SecurityAlgorithms.HmacSha256);

            var dateTime = UnixMillisecondsToDateTimeUtc(claims.FindFirst(ClaimTypes.Expiration)?.Value) ??
                           DateTime.Now.AddDays(1);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = dateTime,
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        
        

        public static bool IsJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken is not null &&
                       jwtToken.Claims.Any(s => s.Type.Equals("nameid", StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        public static DateTime? UnixMillisecondsToDateTimeUtc(string? unixMilliseconds)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (long.TryParse(unixMilliseconds, out var expiration))
            {
                return dateTime.AddMilliseconds(expiration);
            }
            else return null;
        }
    }

