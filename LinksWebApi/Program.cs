
using LinksWebApi.BL.Interfaces;
using LinksWebApi.BL.Services;
using LinksWebApi.Data;
using LinksWebApi.Data.Interfaces;
using LinksWebApi.Data.Repositories;
using LinksWebApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using LinksWebApi.BL.Dto.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using LinksWebApi.Swagger;
using LinksWebApi.Swagger.Examples.Dto;

namespace LinksWebApi
{
    /// <summary>
    /// Главная точка входа в приложение.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Главная точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        public static void Main(string[] args)
        {
            // Создание и конфигурация веб-приложения
            var builder = WebApplication.CreateBuilder(args);

            // Получение информации об окружении
            var environment = builder.Environment;

            // Регистрация контроллеров
            builder.Services.AddControllers();

            if (environment.IsDevelopment())
            {
                // Настройка Swagger для документирования API
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                    c.ExampleFilters();

                    // Настройка Swagger для использования JWT
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Введите 'Bearer' [пробел] и затем ваш JWT токен.\nПример: \"Bearer 12345abcdef\"",
                    });
                    c.OperationFilter<AuthorizeAttributesToSwaggerFilter>();
                });

                builder.Services.AddSwaggerExamplesFromAssemblyOf<SmartLinkBaseDtoExample>();
            }

            // Настройка AutoMapper
            builder.Services.AddAutoMapper(typeof(BL.MappingProfile));

            // Регистрация сервисов бизнес-логики
            builder.Services.AddTransient<ISmartLinkService, SmartLinkService>();
            builder.Services.AddTransient<IRedirectionRuleService, RedirectionRuleService>();
            builder.Services.AddTransient<IJwtAuthenticationService, JwtAuthenticationService>();

            // Регистрация репозиториев
            builder.Services.AddTransient<ISmartLinkRepository, SmartLinkRepository>();
            builder.Services.AddTransient<IRedirectionRuleRepository, RedirectionRuleRepository>();

            // Настройка подключения к базе данных
            builder.Services.AddDbContext<LinksDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Конфигурация валидации
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<SmartLinkBaseDtoValidator>();

            // Конфигурация аутентификации
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]
                ?? throw new ApplicationException("Отсутствует конфигурация JWT")); // Используйте безопасный способ хранения ключей
            builder.Services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true; // В продакшне должно быть true
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            var app = builder.Build();

            // Конфигурация конвейера запросов HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Применение миграций базы данных при запуске приложения
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<LinksDbContext>();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Не удалось применить миграции базы данных");

                throw;
            }

            // Настройка промежуточных слоев (middleware)
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RedirectorMiddleware>();

            // Настройка маршрутизации для контроллеров
            app.MapControllers();

            // Запуск приложения
            app.Run();
        }
    }
}
