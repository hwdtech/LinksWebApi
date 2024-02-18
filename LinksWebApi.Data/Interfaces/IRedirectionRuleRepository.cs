
using LinksWebApi.Data.Entities;

namespace LinksWebApi.Data.Interfaces
{
    public interface IRedirectionRuleRepository
    {
        Task<LanguageRedirectionRule> CreateAsync(LanguageRedirectionRule entity);

        Task<TimeRedirectionRule> CreateAsync(TimeRedirectionRule entity);
    }
}
