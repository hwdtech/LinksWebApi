using FluentValidation;

namespace LinksWebApi.BL.Dto.Validation
{
    public class LanguageRedirectionRuleCreateDtoValidator : AbstractValidator<LanguageRedirectionRuleCreateDto>
    {
        public LanguageRedirectionRuleCreateDtoValidator()
        {
            // Проверка имени
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя не должно быть пустым")
                .Length(1, 50).WithMessage("Имя должно содержать от 1 до 50 символов");

            // Проверка URL назначения
            RuleFor(x => x.DestinationUrl)
                .NotEmpty().WithMessage("URL назначения не должен быть пустым")
                .Must(BeAValidUrl).WithMessage("Недействительный URL назначения");

            // Проверка языкового тега
            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Язык не должен быть пустым")
                .Matches("^[a-zA-Z]{2}(-[a-zA-Z]{2})?$").WithMessage("Язык должен быть в формате ISO, например 'en' или 'en-US'");
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
