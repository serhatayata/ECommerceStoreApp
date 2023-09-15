namespace Monitoring.BackgroundTasks.Utilities.Results;

public class SuccessDataResult<T> : DataResult<T>
{
    public SuccessDataResult(T data, object message, int statusCode = 200) : base(data, true, message, statusCode)
    {
    }

    public SuccessDataResult(T data) : base(data, true, null, 200)
    {
    }

    // Aşağıdakiler genelde kullanılmıyor.
    public SuccessDataResult(object message, int statusCode = 200) : base(default, true, message, statusCode)
    {

    }

    public SuccessDataResult() : base(default, true)
    {

    }
}
