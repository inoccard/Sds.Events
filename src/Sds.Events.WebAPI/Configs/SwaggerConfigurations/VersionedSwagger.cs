using IATec.DocSearch.Api.SwaggerConfigurations;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace Sds.Events.WebAPI.Configs.SwaggerConfigurations
{
    public static class VersionedSwagger
    {
        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        public static void AddVersionedSwagger(this IServiceCollection services)
        {
            //add api explorer for versioned API
            services.AddVersionedApiExplorer(options =>
            {
                //group name will be resolved to v1, v1.0, etc
                options.GroupNameFormat = "'v'VVV";
                //version is part of the URL
                options.SubstituteApiVersionInUrl = true;
            });

            //add api versioning service
            services.AddApiVersioning();

            //add swagger json generation
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(p => p.FullName);

                //build service provider here, cause it is not yet available in any other way, and get the version description provider
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                //create a document for each version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var version = typeof(VersionedSwagger).Assembly.GetName().Version;

                    //describe the info
                    var info = new OpenApiInfo
                    {
                        Title = $"DocSearch {description.GroupName}",
                        Version = description.ApiVersion?.ToString(),
                        Description = "Event Api",
                        License = new OpenApiLicense()
                        { Name = $"App Version: {description.ApiVersion.MajorVersion}.{description.ApiVersion.MinorVersion}.{version?.Build}" }
                    };

                    //and say if it is deprecated
                    if (description.IsDeprecated)
                        info.Description += " NOTE: This version has been deprecated";

                    //create using the version name and info
                    options.SwaggerDoc(description.GroupName, info);
                    options.OperationFilter<DefaultHeaderFilter>();
                }

                //set default for non body parameters
                options.ParameterFilter<DefaultParametersFilter>();

                //add security definition
                options.AddSecurityDefinition(IdentityServerAuthenticationDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="apiVersionDescriptionProvider"></param>
        public static void UseVersionedSwagger(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwagger();

            //create the UI for swagger
            app.UseSwaggerUI(
                options =>
                {
                    //one for each version
                    foreach (var groupName in provider.ApiVersionDescriptions.Select(desc => desc.GroupName))
                    {
                        options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json",
                            groupName.ToUpperInvariant());
                        options.RoutePrefix = string.Empty;
                    }
                });
        }
    }
}