namespace NotificationService.Api.Models.Email;

public class EmailWithAttachment : EmailBase
{
    public IFormFileCollection EmailAttachments { get; set; }
}
