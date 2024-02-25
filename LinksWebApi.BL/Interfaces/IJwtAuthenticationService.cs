using LinksWebApi.BL.Dto;

namespace LinksWebApi.BL.Interfaces
{
    public interface IJwtAuthenticationService
    {
        TokenDto? Create(TokenCreateDto tokenDto);
    }
}
