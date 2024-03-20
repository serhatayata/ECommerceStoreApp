using CatalogService.Api.Attributes;
using CatalogService.Api.Infrastructure.Filters;
using CatalogService.Api.Utilities.Providers;
using CatalogService.Api.Utilities.Validations.FluentValidation.Base;
using CatalogService.Api.Utilities.Validations.FluentValidation.BrandValidators;
using CatalogService.Api.Utilities.Validations.FluentValidation.CategoryValidators;
using CatalogService.Api.Utilities.Validations.FluentValidation.CommentValidators;
using CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators;
using CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Text.Json.Serialization;

namespace CatalogService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 5)]
public class ControllerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(FluentValidationCustomValidationAttribute));
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));

            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        }).AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        //API VERSIONING
        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;

            opt.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("x-version"),
                new QueryStringApiVersionReader("api-version")
            );

            opt.ErrorResponses = new ApiVersioningErrorResponseProvider();
        });

        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

        //VALIDATORS
        #region Base
        services.AddValidatorsFromAssemblyContaining<StringModelValidator>();
        services.AddValidatorsFromAssemblyContaining<IntModelValidator>();
        services.AddValidatorsFromAssemblyContaining<BoolModelValidator>();
        #endregion
        #region Brand
        services.AddValidatorsFromAssemblyContaining<BrandAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<BrandUpdateModelValidator>();
        #endregion
        #region Category
        services.AddValidatorsFromAssemblyContaining<CategoryAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<CategoryUpdateModelValidator>();
        #endregion
        #region Comment
        services.AddValidatorsFromAssemblyContaining<CommentAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<CommentUpdateModelValidator>();
        #endregion
        #region Feature
        services.AddValidatorsFromAssemblyContaining<FeatureAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<FeatureUpdateModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductFeatureModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductFeaturePropertyAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductFeaturePropertyUpdateModelValidator>();
        #endregion
        #region Product
        services.AddValidatorsFromAssemblyContaining<PriceBetweenModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductUpdateModelValidator>();
        #endregion

        services.AddFluentValidationAutoValidation(f =>
        {
            f.DisableDataAnnotationsValidation = true;
        });
        //services.AddFluentValidationClientsideAdapters(); // for client side
    }
}
