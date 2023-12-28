using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sds.Events.Domain.Identity;
using Sds.Events.Repository.Data;
using System.Text;

namespace Sds.Events.WebAPI.Configs.Authentication
{
    public static class AuthenticationConfig
    {
        public static void AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        // assinatura da chave do emissor
                        ValidateIssuerSigningKey = true,
                        // config da chave da API
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        SaveSigninToken = true
                    };
                });

            services.AddAuthorization();
        }

        public static void UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static void AddIdentityUser(this IServiceCollection services)
        {
            // todos os controller terão que passar por uma autenticação
            // quem vai consumir a API precisa estar autenticado e autorizado
            // remove as obrigatoriedades padrão de senha
            var builder = services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false; // sem caracteres especiais
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            })
             .AddEntityFrameworkStores<EventsContext>(); // EntityFramework levará em consideração sempre o contexto

            /// <summary>
            /// instancia o IdentityBuilder com o tipo de usuário, tipo de papel e o serviço criado acima
            /// </summary>
            /// <returns></returns>
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);

            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>(); // gerenciador dos papeis
            builder.AddSignInManager<SignInManager<User>>();

        }
    }
}