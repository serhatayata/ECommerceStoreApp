using FileService.Api.Entities;

namespace FileService.Api.Dtos.ImageDtos
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int FileEntityTypeId { get; set; }
        public FileEntityType FileType { get; set; }
        public string EntityId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
