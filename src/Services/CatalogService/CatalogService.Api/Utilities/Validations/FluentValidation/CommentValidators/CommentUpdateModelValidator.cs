using CatalogService.Api.Models.CommentModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CommentValidators
{
    public class CommentUpdateModelValidator : AbstractValidator<CommentUpdateModel>
    {
        public CommentUpdateModelValidator()
        {
            
        }
    }
}
