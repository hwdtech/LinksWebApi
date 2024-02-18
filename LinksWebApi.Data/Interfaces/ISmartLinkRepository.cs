using LinksWebApi.Data.Entities;

namespace LinksWebApi.Data.Interfaces
{
    public interface ISmartLinkRepository
    {
        Task<SmartLink> CreateAsync(SmartLink smartLink);
        Task<SmartLink?> GetByIdAsync(int id);
        Task UpdateAsync(SmartLink smartLink);
        Task<bool> DeleteAsync(int id);
        Task<SmartLink?> GetByRelativePathAsync(string relativePath);
    }
}
