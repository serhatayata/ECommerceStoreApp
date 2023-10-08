namespace Localization.BackgroundTasks.Models;

public class Resource
{
    public int Id { get; set; }
    public int LanguageId { get; set; }
    public int MemberId { get; set; }
    public string Tag { get; set; }
    public string ResourceCode { get; set; }
    public long CreateDate { get; set; }
    public string LanguageCode { get; set; }
    public string Value { get; set; }
    public bool Status { get; set; }
}
