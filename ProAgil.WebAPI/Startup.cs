using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ProAgil.Repository.Data;

namespace ProAgil.WebAPI {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            /// <summary>
            /// Injeção de Dependêncica
            /// </summary>
            /// <param name="("DefaultConnection""></param>
            /// <typeparam name="DataContext"></typeparam>
            /// <returns></returns>
            //services.AddDbContext<Repository.Data.ProAgilContext> (d => d.UseSqlServer (Configuration.GetConnectionString ("DefaultConnection")));
            /// <summary>
            /// Injeção de Dependência do ProAgilRepository
            /// </summary>
            /// <typeparam name="IProAgilRepository"></typeparam>
            /// <typeparam name="ProAgilRepository"></typeparam>
            /// <returns></returns>
            services.AddScoped <IProAgilRepository, ProAgilRepository>();

            services.AddAutoMapper();
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_3_0);
            services.AddCors ();
            //services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseCors (x => x.AllowAnyOrigin ().AllowAnyMethod ().AllowAnyHeader ());
            app.UseHttpsRedirection ();
            app.UseStaticFiles();
            app.UseStaticFiles (new StaticFileOptions(){
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseRouting ();

            //app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}