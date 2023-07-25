namespace CatalogService.Api.Models.CacheModels
{
    public class CacheKeyModel
    {
        public CacheKeyModel()
        {
            
        }

        public CacheKeyModel(
            string projectName,
            string className,
            string methodName,
            string language,
            string prefix,
            string[] parameters)
        {
            this.ProjectName = projectName;
            this.ClassName = className;
            this.MethodName = methodName;
            this.Language = language;
            this.Prefix = prefix;
            this.Parameters = parameters;
        }

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
