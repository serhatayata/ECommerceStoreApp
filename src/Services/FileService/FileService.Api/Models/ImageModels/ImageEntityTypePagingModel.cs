using FileService.Api.Models.Base;

namespace FileService.Api.Dtos.ImageDtos;

public class ImageEntityTypePagingModel : Paging
{
    public int EntityTypeId { get; set; }
}
