using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer.Api.Attributes;
using IdentityServer.Api.Validations.FluentValidation.ClientValidations;
using IdentityServer.Api.Validations.FluentValidation.UserValidations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 8)]
public class ControllerServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment hostEnvironment)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(FluentValidationCustomValidationAttribute));
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        })
        .AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        #region User
        services.AddValidatorsFromAssemblyContaining<UserLoginModelValidator>();
        services.AddValidatorsFromAssemblyContaining<UserAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<UserUpdateModelValidator>();
        #endregion
        #region Client
        services.AddValidatorsFromAssemblyContaining<ClientAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ApiScopeAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ApiResourceAddModelValidator>();
        services.AddValidatorsFromAssemblyContaining<IdentityResourceAddModelValidator>();
        #endregion

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters(); // for client side    }

        return Task.CompletedTask;
    }
}