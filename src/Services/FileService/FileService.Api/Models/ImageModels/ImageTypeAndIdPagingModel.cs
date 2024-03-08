using FileService.Api.Models.Base;

namespace FileService.Api.Models.ImageModels;

public class ImageTypeAndIdPagingModel : Paging
{
    public int EntityType { get; set; }
    public int EntityId { get; set; }
}
