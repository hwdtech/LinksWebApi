
namespace LinksWebApi.BL.Dto
{
    public record LanguageRedirectionRuleCreateDto(string Name, string DestinationUrl, string Language);

    public record LanguageRedirectionRuleDto(int? Id, string Name, string DestinationUrl, string Language) :
        RedirectionRuleDto(Id, Name, DestinationUrl, null);
}
