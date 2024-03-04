using FileService.Api.Models.Base;

namespace FileService.Api.Dtos.ImageDtos;

public class ImageEntityTypeAndIdPagingModel : Paging
{
    public int EntityType { get; set; }
    public int EntityId { get; set; }
}
