using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            /// Injeção de Dependência do ProAgilRepository
            /// </summary>
            /// <returns></returns>
            services.AddScoped<IProAgilRepository, ProAgilRepository>();
        }

        public static void AddIdentityUser(this IServiceCollection services)
        {
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
            /// instancia o IdentityBuilder com o tipo de usuário, tipo de papel e o serviço criado acima
            /// </summary>
            /// <returns></returns>
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);

            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>(); // gerenciador dos papeis
            builder.AddSignInManager<SignInManager<User>>();

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
        }
    }
}