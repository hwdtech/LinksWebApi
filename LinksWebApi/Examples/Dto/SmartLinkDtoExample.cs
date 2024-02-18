using LinksWebApi.BL.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace LinksWebApi.Examples.Dto
{
    /// <summary>
    /// Предоставляет примеры для объекта SmartLinkBaseDto, используемых в Swagger UI.
    /// Этот класс используется Swashbuckle для генерации примеров объектов SmartLinkBaseDto,
    /// которые отображаются в документации Swagger.
    /// </summary>
    public class SmartLinkBaseDtoExample : IExamplesProvider<SmartLinkBaseDto>
    {
        /// <summary>
        /// Генерирует и возвращает пример объекта SmartLinkBaseDto.
        /// Метод используется Swashbuckle для создания образца данных в Swagger UI.
        /// </summary>
        /// <returns>Пример объекта SmartLinkBaseDto.</returns>
        public SmartLinkBaseDto GetExamples()
        {
            return new SmartLinkBaseDto("/my-smart-link", "Моя умная ссылка");
        }
    }
}
