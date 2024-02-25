using LinksWebApi.BL.Dto;
using LinksWebApi.BL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinksWebApi.BL.Services
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private const string JwtKeyPropertyPath = "Jwt:Key";
        private const string JwtExpirationPropertyPath = "Jwt:ExpirationDays";

        private readonly IConfiguration _configuration;
        private readonly int _expirationDays;

        public JwtAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _expirationDays = _configuration.GetValue<int>(JwtExpirationPropertyPath);
        }

        private string GenerateJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration[JwtKeyPropertyPath] ?? throw new ApplicationException("Отсутствует конфигурация JWT"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(_expirationDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public TokenDto? Create(TokenCreateDto tokenDto)
        {
            // Здесь должна быть логика проверки учетных данных пользователя
            // Этот пример просто проверяет фиктивные данные
            if (tokenDto is { UserName: "user", Password: "pass" })
            {
                var token = GenerateJwtToken(tokenDto.UserName);
                return new TokenDto(token);
            }

            return null;
        }
    }
}
