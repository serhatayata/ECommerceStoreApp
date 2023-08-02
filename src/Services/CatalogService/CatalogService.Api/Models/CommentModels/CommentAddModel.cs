using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.CommentModels
{
    public class CommentAddModel : IModel
    {
        /// <summary>
        /// Product id of the comment
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// User of the comment
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Content of the comment
        /// </summary>
        public string Content { get; set; }
    }
}
