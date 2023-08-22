﻿namespace CatalogService.Api.Models.Identity;

public class PermissionModel
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string[] Scope { get; set; }
    public int Duration { get; set; }
}
