
namespace LinksWebApi.BL.Dto
{
    public record TimeRedirectionRuleCreateDto(string Name, string DestinationUrl, DateTimeOffset? IntervalStart, DateTimeOffset? IntervalEnd);

    public record TimeRedirectionRuleDto(int? Id, string Name, string DestinationUrl, DateTimeOffset? IntervalStart, DateTimeOffset? IntervalEnd) :
        RedirectionRuleDto(Id, Name, DestinationUrl, null);
}
