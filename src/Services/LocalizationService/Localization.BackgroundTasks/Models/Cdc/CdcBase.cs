namespace Localization.BackgroundTasks.Models.Cdc;

public class CdcBase<T>
{
    public CdcSchema Schema { get; set; }
    public CdcPayload<T> Payload { get; set; }
}
