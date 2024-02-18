using LinksWebApi.BL.Dto;
using Microsoft.AspNetCore.Http;

namespace LinksWebApi.BL.Interfaces
{
    public interface IRedirectionRuleService
    {
        Task<LanguageRedirectionRuleDto> Create(int smartLinkId, LanguageRedirectionRuleCreateDto dto);

        Task<TimeRedirectionRuleDto> Create(int smartLinkId, TimeRedirectionRuleCreateDto dto);

        string? TestRule(RedirectionRuleDto ruleDto, HttpRequest httpRequest);
    }
}
