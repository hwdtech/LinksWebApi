using LinksWebApi.BL.Dto;

namespace LinksWebApi.BL.Interfaces;

public interface ISmartLinkService
{
    Task<SmartLinkDto> Create(SmartLinkBaseDto dto);
    Task<SmartLinkDto?> GetById(int id);
    Task<SmartLinkDto?> Update(int id, SmartLinkBaseDto dto);
    Task<bool> Delete(int id);
    Task<SmartLinkDto?> Find(string relativePath);
}