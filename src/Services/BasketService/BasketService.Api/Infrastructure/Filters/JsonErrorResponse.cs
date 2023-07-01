namespace BasketService.Api.Infrastructure.Filters
{
    public class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMessage { get; set; }
    }
}