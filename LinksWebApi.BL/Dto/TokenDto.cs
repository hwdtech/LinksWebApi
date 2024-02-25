
namespace LinksWebApi.BL.Dto
{
    public record TokenCreateDto(string UserName, string Password);

    public record TokenDto(string Token);
}
