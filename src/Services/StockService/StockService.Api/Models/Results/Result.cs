namespace StockService.Api.Utilities.Results;

public class Result
{
    public Result(bool success, object message, int statusCode):this(success)
    {
        Message = message;
        StatusCode = statusCode;
        //Success = success; => Bunun yerine yokarıdaki ":this(success)" kullanıldı.
    }

    public Result(bool success, string messageCode="")
    {
        Success = success;
        StatusCode = success ? 200 : 400;
        Message = success ? "Successful" : "Operation Failed";
        MessageCode = messageCode;
    }
    public bool Success { get; }
    public object Message { get; }
    public int StatusCode { get; }
    public string MessageCode { get; }

}
