using FluentValidation;

namespace MyCommerce.Business.Models.Validations
{
    public class ProviderValidation : AbstractValidator<Provider>
    {
        public ProviderValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("O campo nome precisa ser fornecido")
                .Length(2, 100).WithMessage("O campo nome precisa ter entre {MinLength} e {MaxLength} caracteres");

            When(p => p.ProviderType == ProviderType.LegalPerson, () =>
            {
                RuleFor(p => p.Document.Length).Equal(11).WithMessage("O campo documento precisa ter {ComparisonValue} caracterses e foi fornecido {PropertyValue}");
            });

            When(p => p.ProviderType == ProviderType.PhysicalPerson, () =>
            {
                RuleFor(p => p.Document.Length).Equal(14).WithMessage("O campo documento precisa ter {ComparisonValue} caracterses e foi fornecido {PropertyValue}");
            });
        }
        
    }
}
