namespace Localization.BackgroundTasks.Models.Cdc;

public class CdcSchema
{
    public string Type { get; set; }
    public List<CdcField> Fields { get; set; }
    public bool Optional { get; set; }
    public string Name { get; set; }
    public int Version { get; set; }
}
