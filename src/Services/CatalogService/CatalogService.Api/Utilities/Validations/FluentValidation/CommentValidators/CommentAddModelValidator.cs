using CatalogService.Api.Models.CommentModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CommentValidators
{
    public class CommentAddModelValidator : AbstractValidator<CommentAddModel>
    {
        public CommentAddModelValidator()
        {
            RuleFor(b => b.ProductId).NotEmpty().NotNull().WithMessage("Product Id cannot be empty");
            RuleFor(b => b.ProductId).GreaterThan(0).WithMessage("Product Id must be greater than 0");

            RuleFor(b => b.Content).NotEmpty().NotNull().WithMessage("Content cannot be empty");
            RuleFor(b => b.Content).Length(2,1000).WithMessage("Product Id must be greater than 0");

            RuleFor(b => b.UserId).NotEmpty().NotNull().WithMessage("User Id cannot be empty");
        }
    }
}