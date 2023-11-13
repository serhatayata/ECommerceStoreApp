namespace PaymentService.Api.Models.Settings;

public class QueueSettings
{
    /// <summary>
    /// Username of the queue login
    /// </summary>
    public string Username { get; set; }
    /// <summary>
    /// Password of the queue login
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// Host address of the queue
    /// </summary>
    public string Host { get; set; }
    /// <summary>
    /// Virtual host
    /// </summary>
    public string VirtualHost { get; set; }
    /// <summary>
    /// Port of the queue
    /// </summary>
    public int Port { get; set; }
}
