namespace CatalogService.Api.Utilities.Results
{
    public class JsonErrorResult : Result
    {
        public JsonErrorResult(string[] message) : 
            base(success: false, message: message, statusCode: StatusCodes.Status400BadRequest)
        {
            
        }

        public object DeveloperMessage { get; set; }
    }
}
