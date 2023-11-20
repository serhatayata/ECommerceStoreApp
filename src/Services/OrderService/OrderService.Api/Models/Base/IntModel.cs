namespace OrderService.Api.Models.Base;

public class IntModel : IModel
{
    public IntModel()
    {

    }

    public IntModel(int value)
    {
        this.Value = value;
    }

    public int Value { get; set; }
}
