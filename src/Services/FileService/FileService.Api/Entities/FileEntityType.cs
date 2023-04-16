namespace FileService.Api.Entities
{
    /// <summary>
    /// Entity type of the file
    /// </summary>
    public class FileEntityType
    {
        /// <summary>
        /// Id of file type
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the file type
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the file type
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Images of the file type
        /// </summary>
        public virtual ICollection<Image> Images { get; set; }
    }
}
