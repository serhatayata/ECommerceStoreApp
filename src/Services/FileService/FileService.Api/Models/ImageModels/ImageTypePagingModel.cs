using FileService.Api.Models.Base;

namespace FileService.Api.Models.ImageModels;

public class ImageTypePagingModel : Paging
{
    public ImageType Type { get; set; }
}
