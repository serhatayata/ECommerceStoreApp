namespace Localization.BackgroundTasks.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InstallerOrderAttribute : Attribute
{
    public int Order { get; set; }
}
