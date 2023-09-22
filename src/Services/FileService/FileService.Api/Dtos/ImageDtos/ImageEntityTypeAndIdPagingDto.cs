using FileService.Api.Models.Base;

namespace FileService.Api.Dtos.ImageDtos;

public class ImageEntityTypeAndIdPagingDto : Paging
{
    public int EntityType { get; set; }
    public int EntityId { get; set; }
}
