using Shared.Utilities.Models;

namespace FileService.Api.Dtos.ImageDtos
{
    public class ImageEntityTypePagingDto : Paging
    {
        public int EntityTypeId { get; set; }
        public override int Page { get; set; } = 1;
        public override int PageSize { get; set; } = 8;
    }
}
