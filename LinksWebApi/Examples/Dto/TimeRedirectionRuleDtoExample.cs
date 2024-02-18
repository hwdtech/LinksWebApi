using LinksWebApi.BL.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace LinksWebApi.Examples.Dto
{
    /// <summary>
    /// Предоставляет примеры для объекта TimeRedirectionRuleCreateDto, используемых в Swagger UI.
    /// Этот класс используется Swashbuckle для генерации примеров объектов TimeRedirectionRuleCreateDto,
    /// которые отображаются в документации Swagger.
    /// </summary>
    public class TimeRedirectionRuleDtoExample : IExamplesProvider<TimeRedirectionRuleCreateDto>
    {
        /// <summary>
        /// Генерирует и возвращает пример объекта LanguageRedirectionRuleCreateDto.
        /// Метод используется Swashbuckle для создания образца данных в Swagger UI.
        /// </summary>
        /// <returns>Пример объекта LanguageRedirectionRuleCreateDto.</returns>
        public TimeRedirectionRuleCreateDto GetExamples()
        {
            return new TimeRedirectionRuleCreateDto("Поисковик Google", "https://google.com", DateTimeOffset.Now.AddMinutes(2), DateTimeOffset.Now.AddMinutes(22));
        }
    }
}
