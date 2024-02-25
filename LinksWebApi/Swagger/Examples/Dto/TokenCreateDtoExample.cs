using LinksWebApi.BL.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace LinksWebApi.Swagger.Examples.Dto
{
    /// <summary>
    /// Предоставляет примеры для объекта TokenCreateDto, используемых в Swagger UI.
    /// Этот класс используется Swashbuckle для генерации примеров объектов TokenCreateDto,
    /// которые отображаются в документации Swagger.
    /// </summary>
    public class TokenCreateDtoExample : IExamplesProvider<TokenCreateDto>
    {
        /// <summary>
        /// Генерирует и возвращает пример объекта TokenCreateDto.
        /// Метод используется Swashbuckle для создания образца данных в Swagger UI.
        /// </summary>
        /// <returns>Пример объекта TokenCreateDto.</returns>
        public TokenCreateDto GetExamples()
        {
            return new TokenCreateDto("user", "pass");
        }
    }
}
