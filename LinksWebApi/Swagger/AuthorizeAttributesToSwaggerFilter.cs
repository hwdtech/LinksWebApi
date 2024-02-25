using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LinksWebApi.Swagger
{
    /// <summary>
    /// Класс фильтра операций для добавления требований безопасности к Swagger документации.
    /// </summary>
    public class AuthorizeAttributesToSwaggerFilter : IOperationFilter
    {
        /// <summary>
        /// Применяет фильтр безопасности к операции Swagger.
        /// </summary>
        /// <param name="operation">Операция Swagger, к которой применяется фильтр.</param>
        /// <param name="context">Контекст фильтра операций.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Получение политик авторизации, заданных для методов контроллера
            var controllerAuthorizeAttributes = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .ToList();

            var actionAuthorizeAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .ToList();

            var requiredScopes = controllerAuthorizeAttributes?.Union(actionAuthorizeAttributes)
                .Select(attr => attr.Policy)
                .Distinct().ToList();

            // Если политики не заданы, возврат без изменений
            if (requiredScopes == null || requiredScopes.Count == 0)
            {
                return;
            }

            // Добавление ответов для неавторизованных и запрещенных запросов
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            // Определение схемы безопасности для операции
            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };

            // Назначение требований безопасности для операции
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [ oAuthScheme ] = requiredScopes.ToList()
                }
            };
        }
    }
}
