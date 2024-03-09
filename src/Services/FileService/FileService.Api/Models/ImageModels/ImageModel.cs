namespace FileService.Api.Models.ImageModels;

public class ImageModel
{
    public int Id { get; set; }
    public IFormFile Data { get; set; }
    public int FileUserId { get; set; }
    public string Path { get; set; }
    public string EntityId { get; set; }
    public ImageType Type { get; set; }
    public DateTime CreateDate { get; set; }
}
