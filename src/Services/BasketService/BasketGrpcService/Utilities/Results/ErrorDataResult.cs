using System;
using System.Collections.Generic;
using System.Text;

namespace BasketGrpcService.Utilities.Results
{
    public class ErrorDataResult<T>:DataResult<T>
    {
        public ErrorDataResult(T data, object message, int statusCode=400) : base(data, false, message, statusCode)
        {
        }

        public ErrorDataResult(T data) : base(data, false, null, 400)
        {
        }

        // Aşağıdakiler genelde kullanılmıyor.
        public ErrorDataResult(object message, int statusCode=400) : base(default, false, message, statusCode)
        {

        }

        public ErrorDataResult() : base(default, false)
        {

        }
    }
}
