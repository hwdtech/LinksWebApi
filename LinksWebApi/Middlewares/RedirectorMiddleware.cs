using LinksWebApi.BL.Interfaces;

namespace LinksWebApi.Middlewares
{
    /// <summary>
    /// Middleware для редиректа на основе пользовательских правил.
    /// </summary>
    public class RedirectorMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RedirectorMiddleware"/>.
        /// </summary>
        /// <param name="next">Следующий делегат в конвейере запросов.</param>
        public RedirectorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Обрабатывает HTTP-запрос, выполняя редирект на основе пользовательских правил редиректа.
        /// </summary>
        /// <param name="context">Контекст HTTP запроса.</param>
        /// <param name="smartLinkService">Сервис для работы со Smart Links.</param>
        /// <param name="redirectionRuleService">Сервис для проверки и применения правил редиректа.</param>
        public async Task InvokeAsync(HttpContext context, ISmartLinkService smartLinkService, IRedirectionRuleService redirectionRuleService)
        {
            await _next(context);

            if (context.Response.StatusCode != 404)
            {
                return;
            }
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                return;
            }

            // Запрос не соответствует никакому маршруту

            var relativePath = context.Request.Path.ToString();
            var smartLink = await smartLinkService.Find(relativePath);

            if (smartLink?.RedirectionRules == null)
            {
                return;
            }

            foreach(var rule in smartLink.RedirectionRules)
            {
                var destinationUrl = redirectionRuleService.TestRule(rule, context.Request);
                if (destinationUrl != null)
                {
                    context.Response.Redirect(destinationUrl);
                }
            }
        }
    }
}
