using System;
using System.IO;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using ProAgil.Domain.Identity;
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
            /// <param name="("DefaultConnection")"></param>
            /// <typeparam name="ProAgilContext"></typeparam>
            /// <returns></returns>
            services.AddDbContext<ProAgilContext>(d => d.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<ProAgilContext> (d => d.UseSqlite (Configuration.GetConnectionString ("DefaultConnectionSqlite")));

            /// <summary>
            /// Injeção de Dependência do ProAgilRepository
            /// </summary>
            /// <typeparam name="IProAgilRepository"></typeparam>
            /// <typeparam name="ProAgilRepository"></typeparam>
            /// <returns></returns>
            services.AddScoped<IProAgilRepository, ProAgilRepository>();

            // todos os controller terão que passar por uma autenticação
            // quem vai consumir a API precisa estar autenticado e autorizado
            // remove as obrigatoriedades padrão de senha
            IdentityBuilder builder = services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false; // sem caracteres especiais
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            })
                .AddEntityFrameworkStores<ProAgilContext>(); // EntityFramework levará em consideração sempre o contexto

            /// <summary>
            /// instancia o IdentityBuilder com o tipo de usuário, tippo de papel e o serviço criado acima
            /// </summary>
            /// <returns></returns>
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);

            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>(); // gerenciador dos papeis
            builder.AddSignInManager<SignInManager<User>>();

            // JWT Config
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        // assinatura da chave do emissor
                        ValidateIssuerSigningKey = true,
                        // config da chave da API
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false

                    };
                });
            services.AddAuthorization();
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

            services.AddAutoMapper();
            services.AddCors();
            services.AddControllers(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
           else
                app.UseHsts();

            app.UseAuthentication();

            //app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseMvc();

            app.UseCors();
            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}