namespace FileService.Api.Extensions;

public class RandomExtensions
{
    public static int GetRandomNumber()
    {
        Random random = new();
        return random.Next(0, int.MaxValue);
    }
}
