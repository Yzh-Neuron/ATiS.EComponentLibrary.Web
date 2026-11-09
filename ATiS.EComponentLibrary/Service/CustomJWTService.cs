using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.Dto.User;
using Model.Other;

namespace Service
{
    public class CustomJWTService : ICustomJWTService
    {
        private readonly IOptionsMonitor<JWTTokenOptions> _jwtOptions;

        public CustomJWTService(IOptionsMonitor<JWTTokenOptions> options)
        {
            _jwtOptions = options;
        }

        public async Task<string> GetToken(UserRes user)
        {
            var result =await Task.Run(() =>
            {
                var option = _jwtOptions.CurrentValue;
                var claims = new[]{
                new Claim("Id", user.Id),
                new Claim("NickName", user.NickName),
                new Claim("Name", user.Name),
                new Claim("UserType", user.UserType.ToString()),
                new Claim("Image", user.Image==null?"":user.Image)};

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.SecurityKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: option.Issuer,
                    audience: option.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(10), // 过期时间
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            });

            return result;
        }
    }
}
