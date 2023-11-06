using OrderService.Api.Models.Base;

namespace OrderService.Api.Models.OrderModels;

public class AddressModel : IModel
{
    public string Line { get; set; }

    public string Province { get; set; }

    public string District { get; set; }
}
