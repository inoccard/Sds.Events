using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Sds.Events.Repository.Data;
using Sds.Events.WebAPI.Configs.App;
using Sds.Events.WebAPI.Configs.Authentication;
using Sds.Events.WebAPI.Configs.SwaggerConfigurations;
using System.IO;

namespace Sds.Events.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Injeção de Dependência
            services.AddDbContext<EventsContext>(d => d.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthenticationConfiguration(Configuration);
            services.AddAppServices();
            services.AddIdentityUser();

            services.AddVersionedSwagger();

            services.AddAutoMapper();
            services.AddCors();
            services.AddControllers(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                              IApiVersionDescriptionProvider apiVersionDescriptionProvider,
                              EventsContext dbContext)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseRouting();

            app.UseIdentityConfiguration();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseVersionedSwagger(apiVersionDescriptionProvider);

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}