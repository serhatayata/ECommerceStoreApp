namespace IdentityServer.Api.Models.Localizations
{
    /// <summary>
    /// Resource of the language
    /// </summary>
    public class LocalizationResource
    {
        /// <summary>
        /// Id of the resource localization language
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Display name of the resource language
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Name of the resource language
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Language code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Localizations of the resource language
        /// </summary>
        public ICollection<Localization> Localizations { get; set; }
    }
}
