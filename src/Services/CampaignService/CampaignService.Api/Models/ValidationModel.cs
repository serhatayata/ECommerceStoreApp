namespace CampaignService.Api.Models;

public class ValidationModel<T>
{
    public ValidationModel()
    {
        
    }

    public ValidationModel(T model)
    {
        this.Model = model;
        this.IsSuccess = true;
    }

    public ValidationModel(string[] messages)
    {
        this.ErrorMessages = messages;
        this.IsSuccess = false;
    }

    public bool IsSuccess { get; set; }
    public T Model { get; set; }
    public string[] ErrorMessages { get; set; }
}
