namespace NotificationService.Api.Models.Email;

public class EmailData : EmailBase
{
    public IFormFileCollection EmailAttachments { get; set; }
}
