using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ProAgil.Domain.Identity;
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

            // todos os controller terão que passar por uma autenticação
            // quem vai consumir a API precisa estar autenticado e autorizado
            IdentityBuilder builder = services.AddIdentityCore<User>(options =>
               {
                   options.Password.RequireDigit = false; // sem caracteres especiais
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequireLowercase = false;
                   options.Password.RequireUppercase = false;
                   options.Password.RequiredLength = 4;
               }
            );

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            // EntityFramework levará em consideração sempre o contexto
            builder.AddEntityFrameworkStores<ProAgilContext>();
            builder.AddRoleValidator<Role>();
            builder.AddRoleManager<Role>(); // gerenciador dos papeis
            builder.AddSignInManager<SignInManager<User>>();

            // Determina qual determinado controller será chamado, e adicionado uma política
            services.AddMvc( options =>
            {
                // toda vez que um controller for chamado, deverá respeitar esta política
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser() // requer que o usuário esteja autenticado
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy)); // filtra todas as chamadas do conteoller

            })
            .SetCompatibilityVersion (CompatibilityVersion.Version_3_0);

            services.AddAutoMapper();
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