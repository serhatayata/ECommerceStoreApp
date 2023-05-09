namespace IdentityServer.Api.Models.Localizations
{
    public class Localization
    {
        /// <summary>
        /// Id of the localization
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tag of the localization
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Description of the localization
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Localization code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Id of the resource language
        /// </summary>
        public int LocalizationResourceId { get; set; }
    }
}
