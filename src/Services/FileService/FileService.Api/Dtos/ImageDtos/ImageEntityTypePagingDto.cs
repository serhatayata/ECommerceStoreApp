using FileService.Api.Models.Base;

namespace FileService.Api.Dtos.ImageDtos;

public class ImageEntityTypePagingDto : Paging
{
    public int EntityTypeId { get; set; }
}
