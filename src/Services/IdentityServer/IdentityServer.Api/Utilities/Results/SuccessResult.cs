namespace IdentityServer.Api.Utilities.Results
{
    public class SuccessResult:Result
    {
        public SuccessResult(object message, int statusCode=200) : base(true, message, statusCode)
        {
        }

        public SuccessResult() : base(true,null,200)
        {
        }
    }
}
