namespace FileService.Api.Models.Base;

public class Paging
{
    public virtual int Page { get; set; } = 1;
    public virtual int PageSize { get; set; } = 8;
}
