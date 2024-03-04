using FileService.Api.Entities;

namespace FileService.Api.Dtos.ImageDtos
{
    public class ImageModel
    {
        public int Id { get; set; }
        public IFormFile Data { get; set; }
        public int FileUserId { get; set; }
        public FileUser FileUser { get; set; }
        public string EntityId { get; set; }
        public DateTime CreateDate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
