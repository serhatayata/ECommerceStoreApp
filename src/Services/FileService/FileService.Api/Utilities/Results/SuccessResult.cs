namespace FileService.Api.Utilities.Results;

public class SuccessResult : Result
{
    public SuccessResult(string message) : base(true, message, 200)
    {
    }

    public SuccessResult(string message, int statusCode = 200) : base(true, message, statusCode)
    {
    }

    public SuccessResult(object message, int statusCode = 200) : base(true, message, statusCode)
    {
    }

    public SuccessResult() : base(true, string.Empty, 200)
    {
    }
}

