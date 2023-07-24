namespace CatalogService.Api.Models.CacheModels
{
    public class CacheKeyModel
    {
        /// <summary>
        /// Project name of the source
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Class name of the project
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Method name of the class
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Language of the source method name
        /// </summary>
        public string Language { get; set; } = "tr-TR";

        /// <summary>
        /// Prefix of the source
        /// </summary>
        public string Prefix { get; set; } = string.Empty;

        /// <summary>
        /// Optional parameters
        /// </summary>
        public string[] Parameters { get; set; }
    }
}
