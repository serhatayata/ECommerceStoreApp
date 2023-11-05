namespace OrderService.Api.Models.Base;

public class StringModel
{
    public StringModel()
    {

    }

    public StringModel(string value)
    {
        this.Value = value;
    }

    public string Value { get; set; }
}
