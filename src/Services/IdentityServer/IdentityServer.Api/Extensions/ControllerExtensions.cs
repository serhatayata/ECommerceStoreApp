using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer.Api.Attributes;
using IdentityServer.Api.Validations.FluentValidation.ClientValidations;
using IdentityServer.Api.Validations.FluentValidation.UserValidations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace IdentityServer.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static void AddControllerSettings(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(FluentValidationCustomValidationAttribute));
            }).AddJsonOptions(o =>
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
            services.AddFluentValidationClientsideAdapters(); // for client side
        }
    }
}
