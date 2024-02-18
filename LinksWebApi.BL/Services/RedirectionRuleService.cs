using System.Net.Http.Headers;
using AutoMapper;
using LinksWebApi.BL.Dto;
using LinksWebApi.BL.Interfaces;
using LinksWebApi.Data.Entities;
using LinksWebApi.Data.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LinksWebApi.BL.Services
{
    public class RedirectionRuleService : IRedirectionRuleService
    {
        private readonly IRedirectionRuleRepository _redirectionRuleRepository;
        private readonly IMapper _mapper;

        private readonly Dictionary<string, Func<RedirectionRuleDto, HttpRequest, string?>> _ruleHandlers = new()
        {
            { typeof(TimeRedirectionRule).ToString(), TimeRedirectionRuleHandler },
            { typeof(LanguageRedirectionRule).ToString(), LanguageRedirectionRuleHandler }
        };

        public RedirectionRuleService(IRedirectionRuleRepository redirectionRuleRepository, IMapper mapper)
        {
            _redirectionRuleRepository = redirectionRuleRepository;
            _mapper = mapper;
        }

        private static string? TimeRedirectionRuleHandler(RedirectionRuleDto rule, HttpRequest request)
        {
            if (rule is not TimeRedirectionRuleDto timeRule)
            {
                return null;
            }

            var now = DateTimeOffset.Now;

            if (timeRule.IntervalStart != null && now < timeRule.IntervalStart)
            {
                return null;
            }

            if (timeRule.IntervalEnd != null && now > timeRule.IntervalEnd)
            {
                return null;
            }

            return timeRule.DestinationUrl;
        }

        private static string? LanguageRedirectionRuleHandler(RedirectionRuleDto rule, HttpRequest request)
        {
            if (rule is not LanguageRedirectionRuleDto languageRule)
            {
                return null;
            }

            var acceptLanguageHeader = request.Headers["Accept-Language"].ToString();

            if (string.IsNullOrEmpty(acceptLanguageHeader))
            {
                return null;
            }

            var languages = acceptLanguageHeader.Split(',')
                .Select(StringWithQualityHeaderValue.Parse)
                .OrderByDescending(s => s.Quality.GetValueOrDefault(1))
                .Select(s => s.Value)
                .ToList();

            return languages.Any(l => string.Compare(languageRule.Language, l, StringComparison.OrdinalIgnoreCase) == 0)
                ? languageRule.DestinationUrl : null;
        }

        public async Task<LanguageRedirectionRuleDto> Create(int smartLinkId, LanguageRedirectionRuleCreateDto dto)
        {
            var entity = _mapper.Map<LanguageRedirectionRule>(dto);
            entity.SmartLinkId = smartLinkId;

            await _redirectionRuleRepository.CreateAsync(entity);

            return _mapper.Map<LanguageRedirectionRuleDto>(entity);
        }

        public async Task<TimeRedirectionRuleDto> Create(int smartLinkId, TimeRedirectionRuleCreateDto dto)
        {
            var entity = _mapper.Map<TimeRedirectionRule>(dto);
            entity.SmartLinkId = smartLinkId;

            await _redirectionRuleRepository.CreateAsync(entity);

            return _mapper.Map<TimeRedirectionRuleDto>(entity);
        }

        public string? TestRule(RedirectionRuleDto ruleDto, HttpRequest httpRequest)
        {
            if (ruleDto.TypeInfo != null && _ruleHandlers.TryGetValue(ruleDto.TypeInfo, out var handler))
            {
                return handler(ruleDto, httpRequest);
            }

            return null;
        }
    }
}
