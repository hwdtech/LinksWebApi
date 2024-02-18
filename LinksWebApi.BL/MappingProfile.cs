using AutoMapper;
using LinksWebApi.BL.Dto;
using LinksWebApi.Data.Entities;

namespace LinksWebApi.BL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SmartLink, SmartLinkDto>();
            CreateMap<SmartLinkBaseDto, SmartLink>();
            CreateMap<SmartLinkDto, SmartLink>();

            CreateMap<RedirectionRule, RedirectionRuleDto>()
                .ForMember(r => r.TypeInfo,
                o => o.MapFrom(s => s.GetType().ToString()))
                .IncludeAllDerived();

            CreateMap<LanguageRedirectionRule, LanguageRedirectionRuleDto>();
            CreateMap<LanguageRedirectionRuleDto, LanguageRedirectionRule>();
            CreateMap<LanguageRedirectionRuleCreateDto, LanguageRedirectionRule>();

            CreateMap<TimeRedirectionRule, TimeRedirectionRuleDto>();
            CreateMap<TimeRedirectionRuleDto, TimeRedirectionRule>();
            CreateMap<TimeRedirectionRuleCreateDto, TimeRedirectionRule>();
        }
    }
}
