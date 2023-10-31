namespace NotificationService.Api.Models.Email;

public class OrderSuccessEmail : EmailBase
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string OrderCode { get; set; }
}
