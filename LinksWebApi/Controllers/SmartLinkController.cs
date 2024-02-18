using LinksWebApi.BL.Dto;
using LinksWebApi.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinksWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы со Smart Links.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SmartLinkController : ControllerBase
    {
        private readonly ISmartLinkService _smartLinkService;
        private readonly IRedirectionRuleService _redirectionRuleService;

        /// <summary>
        /// Конструктор
        /// </summary>
        public SmartLinkController(ISmartLinkService smartLinkService, IRedirectionRuleService redirectionRuleService)
        {
            _smartLinkService = smartLinkService;
            _redirectionRuleService = redirectionRuleService;
        }

        /// <summary>
        /// Создает новый Smart Link.
        /// </summary>
        /// <param name="dto">DTO для создания Smart Link.</param>
        /// <returns>Созданный Smart Link.</returns>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /smartlink
        ///     {
        ///        "name": "Моя умная ссылка",
        ///        "originRelativeUrl": "/my-smart-link"
        ///     }
        ///
        /// </remarks>
        [HttpPost("")]
        public async Task<IActionResult> CreateUrl([FromBody] SmartLinkBaseDto dto)
        {
            return Ok(await _smartLinkService.Create(dto));
        }

        /// <summary>
        /// Получает Smart Link по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор Smart Link.</param>
        /// <returns>Smart Link, если он найден; иначе возвращает 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSmartLink(int id)
        {
            var smartLink = await _smartLinkService.GetById(id);
            if (smartLink == null)
            {
                return NotFound();
            }

            return Ok(smartLink);
        }

        /// <summary>
        /// Обновляет существующий Smart Link.
        /// </summary>
        /// <param name="id">Идентификатор Smart Link для обновления.</param>
        /// <param name="dto">DTO с обновленными данными Smart Link.</param>
        /// <returns>Обновленный Smart Link; если Smart Link не найден, возвращает 404 Not Found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSmartLink(int id, [FromBody] SmartLinkBaseDto dto)
        {
            var smartLink = await _smartLinkService.Update(id, dto);
            if (smartLink == null)
            {
                return NotFound();
            }

            return Ok(smartLink);
        }

        /// <summary>
        /// Удаляет Smart Link по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор Smart Link для удаления.</param>
        /// <returns>204 No Content, если Smart Link успешно удален; если Smart Link не найден, возвращает 404 Not Found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSmartLink(int id)
        {
            var result = await _smartLinkService.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Создает правило перенаправления на основе языка для Smart Link.
        /// </summary>
        /// <param name="id">Идентификатор Smart Link.</param>
        /// <param name="dto">DTO для создания правила перенаправления на основе языка.</param>
        /// <returns>Созданное правило перенаправления.</returns>
        [HttpPost("{id}/language-rule")]
        public async Task<IActionResult> CreateLanguageRule(int id, [FromBody] LanguageRedirectionRuleCreateDto dto)
        {
            return Ok(await _redirectionRuleService.Create(id, dto));
        }

        /// <summary>
        /// Создает правило перенаправления на основе времени для Smart Link.
        /// </summary>
        /// <param name="id">Идентификатор Smart Link.</param>
        /// <param name="dto">DTO для создания правила перенаправления на основе времени.</param>
        /// <returns>Созданное правило перенаправления.</returns>
        [HttpPost("{id}/time-rule")]
        public async Task<IActionResult> CreateTimeRule(int id, [FromBody] TimeRedirectionRuleCreateDto dto)
        {
            return Ok(await _redirectionRuleService.Create(id, dto));
        }
    }
}
