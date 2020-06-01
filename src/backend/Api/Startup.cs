using Api.Helpers;
using Application;
using Autofac;
using Core;
using Database.Npgsql;
using Egl.Sit.Api.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;

namespace Api
{
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        protected IConfigurationRoot Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder container)
        {
            container.ConfigureCore(typeof(Startup).Assembly);
            
            container.AddApplication<NpgsqlContext>();

            container.AddUnitOfWork<NpgsqlContext>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllers();
            mvcBuilder.AddControllersAsServices();
            mvcBuilder.AddMvcOptions(config =>
             {
                 //Filtros
                 config.Filters.Add(typeof(HttpGlobalExceptionFilter));
                 config.Filters.Add(typeof(ValidateModelStateFilter));

                 config.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(Point)));
                 config.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(LineString)));
                 config.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(MultiLineString)));
                 config.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(MultiPolygon)));
             });
            mvcBuilder.AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                 foreach (var converter in GeoJsonSerializer.Create(new GeometryFactory(new PrecisionModel())).Converters)
                 {
                     options.SerializerSettings.Converters.Add(converter);
                 }
             });

            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddNpsqlDbContext(Configuration.GetConnectionString("Conn"));

            services.AddSwagger();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(provider, Configuration);
        }
    }
}
