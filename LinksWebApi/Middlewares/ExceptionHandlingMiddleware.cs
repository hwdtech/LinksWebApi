using System.Text.Json;

namespace LinksWebApi.Middlewares
{
    /// <summary>
    /// Middleware для глобальной обработки исключений.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExceptionHandlingMiddleware"/>.
        /// </summary>
        /// <param name="next">Следующий делегат в конвейере запросов.</param>
        /// <param name="logger">Логгер для записи информации об исключениях.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Обрабатывает каждый HTTP запрос.
        /// </summary>
        /// <param name="httpContext">Контекст HTTP запроса.</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Обрабатывает исключения, возникающие во время обработки запроса.
        /// </summary>
        /// <param name="context">Контекст HTTP запроса.</param>
        /// <param name="exception">Исключение, возникшее во время обработки запроса.</param>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }

    /// <summary>
    /// Класс для хранения информации об ошибке.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// HTTP статус-код ошибки.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Возвращает строковое представление объекта ErrorDetails в формате JSON.
        /// </summary>
        public override string ToString() => JsonSerializer.Serialize(this);
    }

}
