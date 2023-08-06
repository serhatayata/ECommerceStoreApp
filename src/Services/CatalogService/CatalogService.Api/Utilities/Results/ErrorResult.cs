namespace CatalogService.Api.Utilities.Results;

public class ErrorResult : Result
{
    public ErrorResult(object message, int statusCode = 400) : base(false, message, statusCode)
    {

    }

    public ErrorResult() : base(false, string.Empty, 400)
    {
    }
}
