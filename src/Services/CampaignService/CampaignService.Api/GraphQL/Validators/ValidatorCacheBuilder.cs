using GraphQL.FluentValidation;

public static class ValidatorCacheBuilder
{
    public static ValidatorInstanceCache Instance;

    public static ValidatorServiceCache InstanceDI;

    static ValidatorCacheBuilder()
    {
        Instance = new();
        Instance.AddValidatorsFromAssembly(typeof(Program).Assembly);

        InstanceDI = new();
        InstanceDI.AddValidatorsFromAssembly(typeof(Program).Assembly);
    }
}