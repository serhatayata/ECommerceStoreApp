using FileService.Api.Models.Base;

namespace FileService.Api.Models.ImageModels;

public class ImageTypeAndFileUserIdPagingModel : Paging
{
    public ImageType Type { get; set; }
    public int FileUserId { get; set; }
}
