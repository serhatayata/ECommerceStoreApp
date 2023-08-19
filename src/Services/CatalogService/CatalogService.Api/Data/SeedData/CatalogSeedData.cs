using AutoMapper;
using CatalogService.Api.Data.Contexts;
using CatalogService.Api.Entities;
using CatalogService.Api.Extensions;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Elastic.Abstract;
using Google.Rpc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polly;
using System.Data.SqlTypes;
using System.Reflection;

namespace CatalogService.Api.Data.SeedData
{
    public class CatalogSeedData
    {
        public async static Task LoadSeedDataAsync(CatalogDbContext context, IServiceScope scope, IWebHostEnvironment env, IConfiguration configuration)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CatalogSeedData>>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            string rootPath = env.ContentRootPath;
            string seedFilePath = Path.Combine(rootPath, "Data", "SeedData", "SeedFiles");

            var policy = Polly.Policy.Handle<SqlException>()
            .Or<SqlAlreadyFilledException>()
            .Or<SqlNullValueException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                                    ex.Message,
                                    nameof(CatalogSeedData),
                                    MethodBase.GetCurrentMethod()?.Name);
            });

            await policy.ExecuteAsync(async () =>
            {
                await context.Database.MigrateAsync();

                if (!await context.Brands.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(Brand));

                    string brandPath = Path.Combine(seedFilePath, $"{nameof(Brand)}.txt");

                    var brandList = File.ReadAllLines(brandPath)
                                          .Skip(1)
                                          .Select(l => l.Split(','))
                                          .Select(l => new Brand()
                                          {
                                              Name = l[0],
                                              Description = l[1]
                                          }).ToList();

                    foreach (var brand in brandList)
                    {
                        if (await context.Brands.AnyAsync(l => l.Name == brand.Name))
                            continue;

                        await context.Brands.AddAsync(brand);
                        await context.SaveChangesAsync();
                    }
                }

                if (!await context.Categories.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(Category));

                    var codeLength = configuration.GetValue<int>("CategoryCodeGenerationLength");

                    string categoryPath = Path.Combine(seedFilePath, $"{nameof(Category)}.txt");

                    var categoryList = File.ReadAllLines(categoryPath)
                                          .Skip(1)
                                          .Select(l => l.Split(','))
                                          .Select(l =>
                                          {
                                              var code = DataGenerationExtensions.RandomCode(codeLength);

                                              return new Category()
                                              {
                                                  Name = l[1],
                                                  Line = int.Parse(l[2]),
                                                  Code = code,
                                                  Link = string.Join("-", DataGenerationExtensions.GenerateLinkData(l[1]), code),
                                                  CreateDate = DateTime.Now,
                                                  UpdateDate = DateTime.Now
                                              };
                                          }).ToList();

                    foreach (var category in categoryList)
                    {
                        if (await context.Categories.AnyAsync(l => l.Code == category.Code))
                            continue;

                        await context.Categories.AddAsync(category);
                        await context.SaveChangesAsync();
                    }
                }

                if (!await context.ProductTypes.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(ProductType));

                    string productTypePath = Path.Combine(seedFilePath, $"{nameof(ProductType)}.txt");

                    var productTypeList = File.ReadAllLines(productTypePath)
                                              .Skip(1)
                                              .Select(l => l.Split(','))
                                              .Select(l => new ProductType()
                                              {
                                                  Name = l[0],
                                                  Description = l[1]
                                              }).ToList();

                    foreach (var productType in productTypeList)
                    {
                        if (await context.ProductTypes.AnyAsync(l => l.Name == productType.Name))
                            continue;

                        await context.ProductTypes.AddAsync(productType);
                        await context.SaveChangesAsync();
                    }
                }

                if (!await context.Features.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(Feature));

                    string featurePath = Path.Combine(seedFilePath, $"{nameof(Feature)}.txt");

                    var featureList = File.ReadAllLines(featurePath)
                                              .Skip(1)
                                              .Select(l => l.Split(','))
                                              .Select(l => new Feature()
                                              {
                                                  Name = l[0]
                                              }).ToList();

                    foreach (var feature in featureList)
                    {
                        if (await context.Features.AnyAsync(l => l.Name == feature.Name))
                            continue;

                        await context.Features.AddAsync(feature);
                        await context.SaveChangesAsync();
                    }
                }

                if (!await context.Products.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(Product));

                    string productPath = Path.Combine(seedFilePath, $"{nameof(Product)}.txt");

                    var currentCategories = await context.Categories.ToListAsync();
                    var currentBrands = await context.Brands.ToListAsync();
                    var currentProductTypes = await context.ProductTypes.ToListAsync();

                    var codeLength = configuration.GetValue<int>("ProductCodeGenerationLength");

                    var productList = File.ReadAllLines(productPath)
                                          .Skip(1)
                                          .Select(l => l.Split(','))
                                          .Select(l =>
                                          {
                                              var rnd = new Random();
                                              var randomDataBrand = rnd.Next(currentBrands.Count());
                                              var randomDataProductType = rnd.Next(currentProductTypes.Count());
                                              var code = DataGenerationExtensions.RandomCode(codeLength);

                                              return new Product()
                                              {
                                                  Name = l[0],
                                                  Description = l[1],
                                                  Price = Convert.ToDecimal(l[2]),
                                                  AvailableStock = int.Parse(l[3]),
                                                  BrandId = currentBrands.Count() > 0 ? currentBrands[randomDataBrand]?.Id : null,
                                                  ProductCode = code,
                                                  Link = string.Join("-", DataGenerationExtensions.GenerateLinkData(l[0]), code),
                                                  ProductTypeId = currentProductTypes.Count() > 0 ? currentProductTypes[randomDataProductType]?.Id : null,
                                                  CreateDate = DateTime.Now,
                                                  UpdateDate = DateTime.Now
                                              };
                                          }).ToList();

                    foreach (var product in productList)
                    {
                        if (await context.Products.AnyAsync(l => l.ProductCode == product.ProductCode))
                            continue;

                        await context.Products.AddAsync(product);
                        await context.SaveChangesAsync();
                    }
                }

                if (!await context.Comments.AnyAsync())
                {
                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(Comment));

                    string commentPath = Path.Combine(seedFilePath, $"{nameof(Comment)}.txt");

                    var currentProducts = await context.Products.ToListAsync();

                    var commentList = File.ReadAllLines(commentPath)
                                          .Skip(1)
                                          .Select(l => l.Split(','))
                                          .Select(l =>
                                          {
                                              var rnd = new Random();
                                              var randomProduct = rnd.Next(currentProducts.Count());

                                              return new Comment()
                                              {
                                                  UserId = l[0],
                                                  Content = l[1],
                                                  Code = Guid.NewGuid().ToString(),
                                                  ProductId = currentProducts[randomProduct].Id,
                                                  CreateDate = DateTime.Now,
                                                  UpdateDate = DateTime.Now
                                              };
                                          }).ToList();

                    foreach (var comment in commentList)
                    {
                        if (await context.Comments.AnyAsync(l => l.Code == comment.Code))
                            continue;

                        await context.Comments.AddAsync(comment);
                        await context.SaveChangesAsync();
                    }
                }

                if (!await context.ProductCategories.AnyAsync())
                {
                    var currentProducts = await context.Products.ToListAsync();
                    var currentCategories = await context.Categories.ToListAsync();

                    if (currentProducts.Count() > 0 && currentCategories.Count() > 0)
                    {
                        foreach (var product in currentProducts)
                        {
                            var rnd = new Random();
                            var randomDataCategory = rnd.Next(currentCategories.Count());

                            var categoryId = currentCategories[randomDataCategory]?.Id ?? default;

                            if (categoryId != default)
                                await context.ProductCategories.AddAsync(new ProductCategory()
                                {
                                    ProductId = product.Id,
                                    CategoryId = categoryId
                                });

                            await context.SaveChangesAsync();
                        }
                    }
                }

                if (!await context.ProductFeatures.AnyAsync())
                {
                    var currentProducts = await context.Products.ToListAsync();
                    var currentFeatures = await context.Features.ToListAsync();

                    if (currentProducts.Count() > 0 && currentFeatures.Count() > 0)
                    {
                        foreach (var product in currentProducts)
                        {
                            var rnd = new Random();
                            var randomData = rnd.Next(1, 4);

                            var featureId = currentFeatures[randomData]?.Id ?? default;

                            if (featureId != default(int))
                                await context.ProductFeatures.AddAsync(new ProductFeature()
                                {
                                    ProductId = product.Id,
                                    FeatureId = featureId
                                });

                            await context.SaveChangesAsync();
                        }
                    }
                }

                if (!await context.ProductFeatureProperties.AnyAsync())
                {

                    logger.LogInformation("Start executing seed data : {ClassName}", nameof(ProductFeatureProperty));

                    string productFeaturePropertyPath = Path.Combine(seedFilePath, $"{nameof(ProductFeatureProperty)}.txt");

                    var productFeatures = await context.ProductFeatures.ToListAsync();

                    var productFeaturePropertyList = File.ReadAllLines(productFeaturePropertyPath)
                                          .Skip(1)
                                          .Select(l => l.Split(','))
                                          .Select(l =>
                                          {
                                              var rnd = new Random();
                                              var randomData = rnd.Next(1, 4);
                                              var currentProductFeature = productFeatures[randomData];

                                              return new ProductFeatureProperty()
                                              {
                                                  ProductFeatureId = currentProductFeature.Id,
                                                  Name = l[0],
                                                  Description = l[1]
                                              };
                                          }).ToList();

                    foreach (var productFeatureProperty in productFeaturePropertyList)
                    {
                        if (await context.ProductFeatureProperties.AnyAsync(l => l.Name == productFeatureProperty.Name))
                            continue;

                        await context.ProductFeatureProperties.AddAsync(productFeatureProperty);
                        await context.SaveChangesAsync();
                    }
                }

                // ELASTIC SEARCH PRODUCTS SEARCH

                var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();
                var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

                var searchIndex = configuration.GetSection("ElasticSearchIndex:Product:Search").Value;
                var indexCreated = await productService.CreateElasticIndex(searchIndex);

                var allProducts = await context.Products.ToListAsync();

                allProducts.ForEach(product =>
                {
                    var productModel = mapper.Map<ProductElasticModel>(product);

                    elasticSearchService.CreateOrUpdateAsync<ProductElasticModel>(searchIndex, productModel);
                });
            });
        }
    }
}
