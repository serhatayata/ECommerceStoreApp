using CatalogService.Api.Models.CommentModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CommentValidators
{
    public class CommentAddModelValidator : AbstractValidator<CommentAddModel>
    {
        public CommentAddModelValidator()
        {
            
        }
    }
}
