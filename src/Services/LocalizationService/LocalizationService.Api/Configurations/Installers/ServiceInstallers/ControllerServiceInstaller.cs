using FluentValidation;
using FluentValidation.AspNetCore;
using LocalizationService.Api.Attributes;
using LocalizationService.Api.Validations.FluentValidation.Base;
using LocalizationService.Api.Validations.FluentValidation.LanguageValidators;
using LocalizationService.Api.Validations.FluentValidation.MemberValidators;
using LocalizationService.Api.Validations.FluentValidation.ResourceValidators;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace LocalizationService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 4)]
public class ControllerServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(FluentValidationCustomValidationAttribute));
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        }).AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        #region Base
        services.AddValidatorsFromAssemblyContaining<StringModelValidator>();
        services.AddValidatorsFromAssemblyContaining<IntModelValidator>();
        services.AddValidatorsFromAssemblyContaining<BoolModelValidator>();
        #endregion
        #region Language
        services.AddValidatorsFromAssemblyContaining<LanguageAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<LanguageUpdateModelValidator>();
        #endregion
        #region Member
        services.AddValidatorsFromAssemblyContaining<MemberAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<MemberUpdateModelValidator>();
        #endregion
        #region Resource
        services.AddValidatorsFromAssemblyContaining<ResourceAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ResourceUpdateModelValidator>();
        #endregion

        services.AddFluentValidationAutoValidation();
        //services.AddFluentValidationClientsideAdapters(); // for client side    }

        return Task.CompletedTask;
    }
}
