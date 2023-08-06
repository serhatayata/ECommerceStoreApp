using CatalogService.Api.Models.CommentModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CommentValidators
{
    public class CommentUpdateModelValidator : AbstractValidator<CommentUpdateModel>
    {
        public CommentUpdateModelValidator()
        {
            RuleFor(b => b.Code).NotEmpty().NotNull().WithMessage("Code cannot be empty");

            RuleFor(b => b.Content).NotEmpty().NotNull().WithMessage("Content cannot be empty");
            RuleFor(b => b.Content).Length(2, 1000).WithMessage("Product Id must be greater than 0");

            RuleFor(b => b.UserId).NotEmpty().NotNull().WithMessage("User Id cannot be empty");
        }
    }
}
