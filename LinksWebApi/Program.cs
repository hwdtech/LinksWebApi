
using LinksWebApi.BL.Interfaces;
using LinksWebApi.BL.Services;
using LinksWebApi.Data;
using LinksWebApi.Data.Interfaces;
using LinksWebApi.Data.Repositories;
using LinksWebApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using LinksWebApi.Examples.Dto;

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
                });
                builder.Services.AddSwaggerExamplesFromAssemblyOf<SmartLinkBaseDtoExample>();
            }

            // Настройка AutoMapper
            builder.Services.AddAutoMapper(typeof(BL.MappingProfile));

            // Регистрация сервисов бизнес-логики
            builder.Services.AddTransient<ISmartLinkService, SmartLinkService>();
            builder.Services.AddTransient<IRedirectionRuleService, RedirectionRuleService>();

            // Регистрация репозиториев
            builder.Services.AddTransient<ISmartLinkRepository, SmartLinkRepository>();
            builder.Services.AddTransient<IRedirectionRuleRepository, RedirectionRuleRepository>();

            // Настройка подключения к базе данных
            builder.Services.AddDbContext<LinksDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
