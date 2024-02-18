
namespace LinksWebApi.BL.Dto
{
    public record SmartLinkBaseDto(string OriginRelativeUrl, string Name);

    public record SmartLinkDto(int Id, string OriginRelativeUrl, string Name, List<RedirectionRuleDto>? RedirectionRules)
        : SmartLinkBaseDto(OriginRelativeUrl, Name);
}
