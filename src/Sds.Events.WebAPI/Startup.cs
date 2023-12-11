using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
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

            services.AddCors(options =>
            {
                options.AddPolicy("Sds.Events.UI", builder =>
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials());
            });

            services.AddAuthenticationConfiguration(Configuration);
            services.AddAppServices();
            services.AddIdentityUser();

            // Determina qual determinado controller será chamado, e adicionando uma política
            // Não é mais necessário colocar autenticação no controller
            services.AddMvc(options =>
            {
                // toda vez que um controller for chamado, deverá respeitar esta política
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser() // requer que o usuário esteja autenticado
                    .Build();
                options
                    .Filters.Add(new AuthorizeFilter(policy)); // filtra todas as chamadas do controller
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddVersionedSwagger();

            services.AddAutoMapper();

            services.AddControllers(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                              IApiVersionDescriptionProvider apiVersionDescriptionProvider,
                              EventsContext dbContext)
        {
            dbContext.Database.Migrate();

            app.UseCors("Sds.Events.UI");
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

            app.UseVersionedSwagger(apiVersionDescriptionProvider);

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}