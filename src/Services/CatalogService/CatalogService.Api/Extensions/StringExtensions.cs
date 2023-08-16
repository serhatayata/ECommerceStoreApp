namespace CatalogService.Api.Extensions
{
    public static class StringExtensions
    {
        public static bool AnyExistsInList(this string current, string[] values)
        {
            if (values == null || values.Length == 0)
                return false;

            var currentValues = current.Split(" ");
            return currentValues.Intersect(values).Any();
        }
    }
}
