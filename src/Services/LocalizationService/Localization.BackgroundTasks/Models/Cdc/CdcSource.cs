namespace Localization.BackgroundTasks.Models.Cdc;

public class CdcSource
{
    public string Version { get; set; }
    public string Connector { get; set; }
    public string Name { get; set; }
    public long Ts_ms { get; set; }
    public string Snapshot { get; set; }
    public string Db { get; set; }
    public object Sequence { get; set; }
    public string Schema { get; set; }
    public string Table { get; set; }
    public string Change_lsn { get; set; }
    public string Commit_lsn { get; set; }
    public int Event_serial_no { get; set; }
}
