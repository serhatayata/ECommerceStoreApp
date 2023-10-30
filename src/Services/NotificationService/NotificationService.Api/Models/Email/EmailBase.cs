namespace NotificationService.Api.Models.Email;

public class EmailBase
{
    public string ToId { get; set; }
    public string ToName { get; set; }
    public string Subject { get; set; }
    public string TextBody { get; set; }
    public string HtmlBody { get; set; }
}
