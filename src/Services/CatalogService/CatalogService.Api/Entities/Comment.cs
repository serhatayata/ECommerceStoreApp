using CatalogService.Api.Entities.Abstract;

namespace CatalogService.Api.Entities
{
    public class Comment : IEntity
    {
        /// <summary>
        /// Id of the comment
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the comment
        /// </summary>
        public string Code { get; set; }
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
        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Surname of the user
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Product entity FK
        /// </summary>
        public Product Product { get; set; }
    }
}
