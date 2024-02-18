
namespace LinksWebApi.Data.Entities
{
    public abstract class RedirectionRule
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string DestinationUrl { get; set; }

        public required int SmartLinkId { get; set; }

        public virtual SmartLink? SmartLink { get; set; }
    }
}
