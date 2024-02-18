
namespace LinksWebApi.Data.Entities
{
    public class SmartLink
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string OriginRelativeUrl { get; set; }

        public required string NormalizedOriginRelativeUrl { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public bool IsEnabled { get; set; } = true;

        public List<RedirectionRule>? RedirectionRules { get; set; }
    }
}
