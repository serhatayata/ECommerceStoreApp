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
            string prefix,
            string[] parameters)
        {
            this.ProjectName = projectName;
            this.ClassName = className;
            this.MethodName = methodName;
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
        /// Prefix of the source
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Optional parameters
        /// </summary>
        public string[] Parameters { get; set; }

        public string[] GetModelValuesAsList()
        {
            var values = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.Prefix))
                values.Add(this.Prefix);

            if (!string.IsNullOrWhiteSpace(this.ProjectName))
                values.Add(this.ProjectName);

            if (!string.IsNullOrWhiteSpace(this.ClassName))
                values.Add(this.ClassName);

            if (!string.IsNullOrWhiteSpace(this.MethodName))
                values.Add(this.MethodName);

            if (this.Parameters != null && this.Parameters.Count() > 0)
                values.AddRange(this.Parameters);

            return values.ToArray();
        }
    }
}
