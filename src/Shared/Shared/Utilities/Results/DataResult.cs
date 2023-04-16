using Newtonsoft.Json;

namespace Shared.Utilities.Results
{
    public class DataResult<T> : Result
    {
        public DataResult(T data, bool success, object message, int statusCode) : base(success, message, statusCode)
        {
            Data = data;
        }

        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        [JsonConstructor]
        public DataResult(T data) : base(true)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
