using LinksWebApi.BL.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace LinksWebApi.Swagger.Examples.Dto
{
    /// <summary>
    /// Предоставляет примеры для объекта LanguageRedirectionRuleCreateDto, используемых в Swagger UI.
    /// Этот класс используется Swashbuckle для генерации примеров объектов LanguageRedirectionRuleCreateDto,
    /// которые отображаются в документации Swagger.
    /// </summary>
    public class LanguageRedirectionRuleCreateDtoExample : IExamplesProvider<LanguageRedirectionRuleCreateDto>
    {
        /// <summary>
        /// Генерирует и возвращает пример объекта LanguageRedirectionRuleCreateDto.
        /// Метод используется Swashbuckle для создания образца данных в Swagger UI.
        /// </summary>
        /// <returns>Пример объекта LanguageRedirectionRuleCreateDto.</returns>
        public LanguageRedirectionRuleCreateDto GetExamples()
        {
            return new LanguageRedirectionRuleCreateDto("Поисковик на русском языке", "https://ya.ru", "ru");
        }
    }
}
