namespace CatalogService.Api.Dtos.Localization;

public class MemberDto
{
    public string Name { get; set; }
    public string MemberKey { get; set; }

    public List<ResourceDto> Resources { get; set; }
}
