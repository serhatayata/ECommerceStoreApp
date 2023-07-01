namespace BasketService.Api.Services.Localization.Abstract
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Gets the string resource with the given name.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <returns>The string resource as a <see cref="string"/>.</returns>
        string this[string culture, string key] { get; }

        /// <summary>
        /// Gets the string resource with the given name and formatted with the supplied arguments.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <param name="arguments">The values to format the string with.</param>
        /// <returns>The formatted string resource as a <see cref="string"/>.</returns>
        string this[string culture, string key, params object[] args] { get; }

        /// <summary>
        /// Gets the string resource value
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="resourceKey"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetStringResource(string culture, string key, params object[] args);

        /// <summary>
        /// Gets the string resource value
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public string GetStringResource(string culture, string key);
    }
}
