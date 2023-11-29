using System.IO;
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
using Proagil.WebAPI.Configs.App;
using Proagil.WebAPI.Configs.SwaggerConfigurations;
using ProAgil.Repository.Data;

namespace ProAgil.WebAPI
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
            /// <summary>
            /// Injeção de Dependêncica
            /// </summary>
            /// <returns></returns>
            services.AddDbContext<ProAgilContext>(d => d.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddAuthenticationConfiguration(Configuration);
            services.AddAppServices();

            services.AddVersionedSwagger();

            services.AddAutoMapper();
            services.AddCors();
            services.AddControllers(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                              IApiVersionDescriptionProvider apiVersionDescriptionProvider,
                              ProAgilContext dbContext)
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

            app.UseMvc();

            app.UseRouting();

            //app.UseIdentityConfiguration();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseVersionedSwagger(apiVersionDescriptionProvider);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}