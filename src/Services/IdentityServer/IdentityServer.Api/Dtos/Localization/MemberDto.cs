namespace IdentityServer.Api.Dtos.Localization
{
    public class MemberDto
    {
        public string Name { get; set; }
        public string MemberKey { get; set; }

        public IReadOnlyList<ResourceDto> Resources { get; set; }
    }
}
