using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sds.Events.Domain.Identity;
using Sds.Events.Repository.Data;

namespace Sds.Events.WebAPI.Configs.App
{
    public static class RegisterAppServicesConfig
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            /// <summary>
            /// Injeção de Dependência do EventsRepository
            /// </summary>
            /// <returns></returns>
            services.AddScoped<IEventsRepository, EventsRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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