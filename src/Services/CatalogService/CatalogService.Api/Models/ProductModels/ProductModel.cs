﻿using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.ProductModels
{
    public class ProductModel : IModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
        public string Link { get; set; }
        public string ProductCode { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BrandId { get; set; }
    }
}
