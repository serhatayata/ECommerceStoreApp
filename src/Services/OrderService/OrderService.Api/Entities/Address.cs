using Microsoft.EntityFrameworkCore;

namespace OrderService.Api.Entities;

[Owned]
public class Address
{
    public string Line { get; set; }

    public string Province { get; set; }

    public string District { get; set; }
}
