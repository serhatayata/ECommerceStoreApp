using MonitoringService.Api.Models.Base.Abstract;

namespace MonitoringService.Api.Models.Base.Concrete;

public class StringModel : IModel
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
