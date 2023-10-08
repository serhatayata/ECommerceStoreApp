namespace Localization.BackgroundTasks.Models.Cdc;

public class CdcRoot<T>
{
    public CdcSchema Schema { get; set; }
    public CdcPayload<T> Payload { get; set; }
}
