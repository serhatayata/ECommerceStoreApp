namespace FileService.Api.Entities
{
    public class Image
    {
        /// <summary>
        /// Id of image
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// FK - File type is actually used for grouping the files, for example Category or product
        /// </summary>
        public int FileEntityTypeId { get; set; }
        public FileEntityType FileType { get; set; }
        /// <summary>
        /// Entity id for example CategoryId or ProductId
        /// </summary>
        public int EntityId { get; set; }
        /// <summary>
        /// URL of the image
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Width of the image
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the image
        /// </summary>
        public int Height { get; set; }
    }
}
