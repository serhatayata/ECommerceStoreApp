namespace IdentityServer.Api.Models.Settings
{
    public class SourceOriginSettings
    {
        public SourceOrigin Development { get; set; }
        public SourceOrigin Product { get; set; }
    }

    public class SourceOrigin
    {
        public string Gateway { get; set; }
        public string Identity { get; set; }
    }
}
