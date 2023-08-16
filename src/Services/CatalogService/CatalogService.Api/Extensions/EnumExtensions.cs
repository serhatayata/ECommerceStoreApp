using CatalogService.Api.Models.KeyParameterModels;

namespace CatalogService.Api.Extensions;

public static class EnumExtensions
{
    public static string GetKeyParameterValue(this EnumKeyParameter parameter)
    {
        var value = parameter.ToString() + "-" + (int)parameter;
        return value;
    }
}
