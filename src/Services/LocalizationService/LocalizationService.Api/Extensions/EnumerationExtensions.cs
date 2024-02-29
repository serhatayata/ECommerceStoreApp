namespace LocalizationService.Api.Extensions;

public static class EnumerationExtensions
{
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> enumerator, int size)
    {
        T[] batch = null;
        var count = 0;

        foreach (var item in enumerator)
        {
            if (batch == null)
                batch = new T[size];

            batch[count++] = item;
            if (count != size)
                continue;

            yield return batch;

            batch = null;
            count = 0;
        }

        if (batch != null && count > 0)
            yield return batch.Take(count).ToArray();
    }
}
