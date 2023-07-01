using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using CatalogService.Api.Attributes;

namespace CatalogService.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static void AddControllerSettings(this IServiceCollection services)
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

            //#region Base
            //services.AddValidatorsFromAssemblyContaining<StringModelValidator>();
            //services.AddValidatorsFromAssemblyContaining<IntModelValidator>();
            //services.AddValidatorsFromAssemblyContaining<BoolModelValidator>();
            //#endregion
            //#region Language
            //services.AddValidatorsFromAssemblyContaining<LanguageAddModelValidator>();
            //services.AddValidatorsFromAssemblyContaining<LanguageUpdateModelValidator>();
            //#endregion
            //#region Member
            //services.AddValidatorsFromAssemblyContaining<MemberAddModelValidator>();
            //services.AddValidatorsFromAssemblyContaining<MemberUpdateModelValidator>();
            //#endregion
            //#region Resource
            //services.AddValidatorsFromAssemblyContaining<ResourceAddModelValidator>();
            //services.AddValidatorsFromAssemblyContaining<ResourceUpdateModelValidator>();
            //#endregion

            services.AddFluentValidationAutoValidation();
            //services.AddFluentValidationClientsideAdapters(); // for client side
        }
    }
}
