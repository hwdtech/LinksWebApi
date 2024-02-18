using LinksWebApi.Data.Entities;
using LinksWebApi.Data.Interfaces;

namespace LinksWebApi.Data.Repositories
{
    public class RedirectionRuleRepository : IRedirectionRuleRepository
    {
        private readonly LinksDbContext _dbContext;

        public RedirectionRuleRepository(LinksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LanguageRedirectionRule> CreateAsync(LanguageRedirectionRule entity)
        {
            _dbContext.RedirectionRules.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TimeRedirectionRule> CreateAsync(TimeRedirectionRule entity)
        {
            _dbContext.RedirectionRules.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
