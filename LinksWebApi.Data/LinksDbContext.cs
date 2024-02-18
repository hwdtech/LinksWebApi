using Microsoft.EntityFrameworkCore;
using LinksWebApi.Data.Entities;

namespace LinksWebApi.Data
{
    public class LinksDbContext : DbContext
    {
        public required DbSet<SmartLink> SmartLinks { get; set; }
        public required DbSet<RedirectionRule> RedirectionRules { get; set; }

        public LinksDbContext()
        {
        }

        public LinksDbContext(DbContextOptions<LinksDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка наследования с использованием TPH
            modelBuilder.Entity<RedirectionRule>()
                .HasDiscriminator<string>("RuleType")
                .HasValue<TimeRedirectionRule>(nameof(TimeRedirectionRule))
                .HasValue<LanguageRedirectionRule>(nameof(LanguageRedirectionRule));
            // Для других наследуемых классов добавьте аналогичные HasValue

            modelBuilder.Entity<SmartLink>()
                .HasIndex(sl => sl.NormalizedOriginRelativeUrl)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}

/*
 * dotnet ef migrations add InitialCreate --project LinksWebApi.Data --startup-project LinksWebApi
 */