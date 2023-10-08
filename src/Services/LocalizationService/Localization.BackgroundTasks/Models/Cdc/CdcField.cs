namespace Localization.BackgroundTasks.Models.Cdc;

public class CdcField
{
    public string Type { get; set; }
    public List<CdcField> Fields { get; set; }
    public bool Optional { get; set; }
    public string Name { get; set; }
    public string Field { get; set; }
    public int? Version { get; set; }
    public object @default { get; set; }
    public CdcParameters Parameters { get; set; }
}
