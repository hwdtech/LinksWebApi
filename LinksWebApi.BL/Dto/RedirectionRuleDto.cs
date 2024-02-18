
using System.Text.Json.Serialization;

namespace LinksWebApi.BL.Dto
{
    [JsonDerivedType(typeof(LanguageRedirectionRuleDto))]
    [JsonDerivedType(typeof(TimeRedirectionRuleDto))]
    public record RedirectionRuleDto(int? Id, string Name, string DestinationUrl, string? TypeInfo);
}
