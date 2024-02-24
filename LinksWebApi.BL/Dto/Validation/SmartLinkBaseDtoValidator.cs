using FluentValidation;

namespace LinksWebApi.BL.Dto.Validation
{
    public class SmartLinkBaseDtoValidator : AbstractValidator<SmartLinkBaseDto>
    {
        public SmartLinkBaseDtoValidator()
        {
            // Правило для проверки, что URL не пуст и соответствует формату URL
            RuleFor(x => x.OriginRelativeUrl)
                .NotEmpty().WithMessage("URL не должен быть пустым")
                .Must(BeAValidUrl).WithMessage("Недействительный URL");

            // Правило для проверки, что имя не пустое и имеет подходящую длину
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя не должно быть пустым")
                .Length(2, 100).WithMessage("Имя должно содержать от 2 до 100 символов");
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Relative, out _);
        }
    }
}
