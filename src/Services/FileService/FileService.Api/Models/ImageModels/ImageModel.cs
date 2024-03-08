using FileService.Api.Entities;

namespace FileService.Api.Models.ImageModels;

public class ImageModel
{
    public int Id { get; set; }
    public IFormFile Data { get; set; }
    public int FileUserId { get; set; }
    public FileUser FileUser { get; set; }
    public string EntityId { get; set; }
    public DateTime CreateDate { get; set; }
}
