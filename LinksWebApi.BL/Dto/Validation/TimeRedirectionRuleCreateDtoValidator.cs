using FluentValidation;

namespace LinksWebApi.BL.Dto.Validation
{
    public class TimeRedirectionRuleCreateDtoValidator : AbstractValidator<TimeRedirectionRuleCreateDto>
    {
        public TimeRedirectionRuleCreateDtoValidator()
        {
            // Проверка имени
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя не должно быть пустым")
                .Length(1, 50).WithMessage("Имя должно содержать от 1 до 50 символов");

            // Проверка URL назначения
            RuleFor(x => x.DestinationUrl)
                .NotEmpty().WithMessage("URL назначения не должен быть пустым")
                .Must(BeAValidUrl).WithMessage("Недействительный URL назначения");

            // Проверка интервала времени
            RuleFor(x => x.IntervalStart)
                .LessThanOrEqualTo(x => x.IntervalEnd)
                .When(x => x.IntervalStart.HasValue && x.IntervalEnd.HasValue)
                .WithMessage("Начало интервала должно быть раньше или равно концу интервала");

            // Опционально: можно добавить дополнительные проверки для IntervalStart и IntervalEnd, если требуется
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
