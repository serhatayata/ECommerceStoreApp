using Shared.Utilities.Models;

namespace FileService.Api.Dtos.ImageDtos
{
    public class ImageEntityTypeAndIdPagingDto : Paging
    {
        public int EntityType { get; set; }
        public int EntityId { get; set; }
        public override int Page { get; set; } = 1;
        public override int PageSize { get; set; } = 8;
    }
}
