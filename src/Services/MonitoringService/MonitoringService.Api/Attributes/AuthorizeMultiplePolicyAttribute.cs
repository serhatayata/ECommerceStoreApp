using Microsoft.AspNetCore.Mvc;
using MonitoringService.Api.Infrastructure.Filters;

namespace MonitoringService.Api.Attributes;

public class AuthorizeMultiplePolicyAttribute : TypeFilterAttribute
{
    public AuthorizeMultiplePolicyAttribute(string policies, bool IsAll) : base(typeof(AuthorizeMultiplePolicyFilter))
    {
        Arguments = new object[] { policies, IsAll };
    }
}
