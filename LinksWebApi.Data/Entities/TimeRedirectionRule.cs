
namespace LinksWebApi.Data.Entities
{
    public class TimeRedirectionRule : RedirectionRule
    {
        public DateTimeOffset? IntervalStart { get; set; }

        public DateTimeOffset? IntervalEnd { get; set; }
    }
}
