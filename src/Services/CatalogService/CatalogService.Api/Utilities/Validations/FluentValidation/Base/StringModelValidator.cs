using CatalogService.Api.Models.Base.Concrete;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.Base
{
    public class StringModelValidator : AbstractValidator<StringModel>
    {
        public StringModelValidator()
        {
            RuleFor(b => b.Value).NotEmpty().WithMessage("Value cannot be empty");
            RuleFor(b => b.Value).NotNull().WithMessage("Value cannot be null");
        }
    }
}
