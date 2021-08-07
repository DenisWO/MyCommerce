using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommerce.Business.Models.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(c => c.PublicPlace).NotEmpty().WithMessage("O campo logradouro precisa ser fornecido")
                .Length(2, 200).WithMessage("O campo logradouro precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.District).NotEmpty().WithMessage("O campo bairro precisa ser fornecido")
                .Length(2, 100).WithMessage("O campo bairro precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.ZipCode).NotEmpty().WithMessage("O campo CEP precisa ser fornecido")
                .Length(8).WithMessage("O campo CEP precisa ter {MaxLength} caracteres");

            RuleFor(c => c.City).NotEmpty().WithMessage("O campo cidade precisa ser fornecido")
                .Length(2, 100).WithMessage("O campo cidade precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.State).NotEmpty().WithMessage("O campo estado precisa ser fornecido")
                .Length(2, 50).WithMessage("O campo estado precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Number).NotEmpty().WithMessage("O campo número precisa ser fornecido")
                .Length(1, 50).WithMessage("O campo número precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
