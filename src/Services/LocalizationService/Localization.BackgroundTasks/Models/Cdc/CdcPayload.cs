namespace Localization.BackgroundTasks.Models.Cdc;

public class CdcPayload<T>
{
    public T Before { get; set; }
    public T After { get; set; }
    public CdcSource Source { get; set; }
    public string Op { get; set; }
    public long Ts_ms { get; set; }
    public object Transaction { get; set; }
}
