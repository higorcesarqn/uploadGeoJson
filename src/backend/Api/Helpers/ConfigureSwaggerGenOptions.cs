using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Linq;

namespace Api.Helpers
{
    public static class ConfigureSwagger
    {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            var path = configuration["SwaggerPath"] ?? "/";
            return
                app.UseSwagger()
                .UseSwaggerUI(
                    options =>
                    {
                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"{path}swagger/{description.GroupName}/swagger.json", $"DNIT - {description.GroupName.ToUpperInvariant()}");
                        }
                    });
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
                .AddSwaggerGen(options =>
                {
                    var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;
                    var applicationName = PlatformServices.Default.Application.ApplicationName;
                    var xmlDocumentPath = Path.Combine(applicationBasePath, $"{applicationName}.xml");

                    if (File.Exists(xmlDocumentPath))
                    {
                        options.IncludeXmlComments(xmlDocumentPath);
                    }

                    options.OperationFilter<SwaggerDefaultValues>();

                    //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    //{
                    //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    //    Name = "Authorization",
                    //    In = ParameterLocation.Header,
                    //    Type = SecuritySchemeType.ApiKey
                    //});

                    //options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    //{
                    //    {
                    //        new OpenApiSecurityScheme
                    //        {
                    //            Reference = new OpenApiReference
                    //            {
                    //                Type = ReferenceType.SecurityScheme,
                    //                Id = "Bearer"
                    //            },
                    //            Scheme = "oauth2",
                    //            Name = "Bearer",
                    //            In = ParameterLocation.Header,

                    //        },
                    //        new List<string>()
                    //    }
                    //});

                });
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerGenOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "API GeoJson",
                Version = description.ApiVersion.ToString(),
                Description = "API do GeoJson",
                Contact = new OpenApiContact() { Name = "Higor César", Email = "higorcesarqn@gmail.com" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " Esta versão da API esta em desuso";
            }

            return info;
        }
    }

    /// <summary>
    /// Representa o filtro de operação Swagger / Swashbuckle usado para documentar o parâmetro implícito da versão da API.
    /// </summary>
    ///<remarks> Isso <see cref = "IOperationFilter" /> é necessário apenas devido a erros no <see cref = "SwaggerGenerator" />.
    /// Depois de corrigidas e publicadas, essa classe pode ser removida. </remarks>
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Aplica o filtro à operação especificada usando o contexto fornecido.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}