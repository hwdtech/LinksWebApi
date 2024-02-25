using LinksWebApi.BL.Dto;
using LinksWebApi.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinksWebApi.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации пользователей.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtAuthenticationService _authenticationService;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="AuthenticationController"/>.
        /// </summary>
        /// <param name="authenticationService">Сервис для аутентификации пользователей.</param>
        public AuthenticationController(IJwtAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Аутентифицирует пользователя и выдает JWT.
        /// </summary>
        /// <param name="tokenDto">DTO с данными для входа пользователя.</param>
        /// <returns>JWT в случае успешной аутентификации.</returns>
        [HttpPost("token")]
        public IActionResult Login([FromBody] TokenCreateDto tokenDto)
        {
            var token = _authenticationService.Create(tokenDto);
            if (token != null)
            {
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
