namespace FileService.Api.Utilities.Results;

public class Result
{
    public Result(bool success, object message, int statusCode) : this(success)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public Result(bool success, string message, int statusCode) : this(success)
    {
        Message = string.IsNullOrWhiteSpace(message) ? 
                  new string[] {} : new string[] { message };
        StatusCode = statusCode;
    }

    public Result(bool success, string[] message, int statusCode) : this(success)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public Result(bool success, string messageCode = "")
    {
        Success = success;
        StatusCode = success ? 200 : 400;
        Message = success ? new string[] { "Successful" } : new string[] { "Operation Failed" };
        MessageCode = messageCode;
    }
    public bool Success { get; }
    public object Message { get; }
    public int StatusCode { get; }
    public string MessageCode { get; }

}
