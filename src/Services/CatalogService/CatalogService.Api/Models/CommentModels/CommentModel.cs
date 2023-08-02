using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.CommentModels
{
    public class CommentModel : IModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
