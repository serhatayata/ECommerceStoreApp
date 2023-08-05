namespace CatalogService.Api.Utilities.Responses
{
    public class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMessage { get; set; } 
    }
}
