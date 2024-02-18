using LinksWebApi.Data.Entities;
using LinksWebApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinksWebApi.Data.Repositories
{
    public class SmartLinkRepository : ISmartLinkRepository
    {
        private readonly LinksDbContext _dbContext;

        public SmartLinkRepository(LinksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SmartLink> CreateAsync(SmartLink smartLink)
        {
            _dbContext.SmartLinks.Add(smartLink);
            await _dbContext.SaveChangesAsync();
            return smartLink;
        }

        public async Task<SmartLink?> GetByIdAsync(int id)
        {
            return await _dbContext.SmartLinks.Include(sl => sl.RedirectionRules)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(SmartLink smartLink)
        {
            _dbContext.SmartLinks.Update(smartLink);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var smartLink = await _dbContext.SmartLinks.FirstOrDefaultAsync(u => u.Id == id);
            if (smartLink == null) return false;

            _dbContext.SmartLinks.Remove(smartLink);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<SmartLink?> GetByRelativePathAsync(string relativePath)
        {
            return await _dbContext.SmartLinks.Include(sl => sl.RedirectionRules)
                .FirstOrDefaultAsync(sl => sl.NormalizedOriginRelativeUrl == relativePath);
        }
    }

}
