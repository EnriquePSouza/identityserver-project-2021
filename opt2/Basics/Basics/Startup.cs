using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Basics.AuthorizationRequirements;
using Basics.Controllers;
using Basics.Transformer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Basics
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Serve para informar onde a autorização precisa ser verificada.
            // Tudo inerente as verificações fica aqui, e onde direcionamos para os logins tbm.
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    // Como não tenho esse cookie criado, ele permite a ação ser axecutada, 
                    // pois não tem nada aprovando ou recusando tal autorização.
                    config.Cookie.Name = "Grandmas.Cookie";
                    // Redireciona sempre que tiver a tag 'authorize' e precisa de aprovação.
                    config.LoginPath = "/Home/Authenticate";
                });

            // Quando vc declara que algo precisa ser authenticado, é essa configuração que acontece no background.
            // A config comentada.
            services.AddAuthorization(config =>
            {
            //     var defaultAuthBuilder = new AuthorizationPolicyBuilder();
            //     var defaultAuthPolicy = defaultAuthBuilder
            //         .RequireAuthenticatedUser()
            //         .RequireClaim(ClaimTypes.DateOfBirth)
            //         .Build();
                
            //     config.DefaultPolicy = defaultAuthPolicy;

            //     config.AddPolicy("Claim.DoB", policyBuilder =>
            //     {
            //         policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
            //     });

                // Role é legado, só se a empresa utilizar, hoje em dia utilizamos uma condição em cima de dados das claims do usuario.
                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

                config.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            // Handler de recursos de base.
            // No OperationsController tem todo o processo de configuração e utilização.
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

            // Para poder chamar as views, a parte de front-end apartir dos controllers.
            // Só declarar o controller com a Herança Controller e não ControllerBase.
            services.AddControllersWithViews(config => {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                    .RequireAuthenticatedUser()
                    .Build();

                // global authorization filter => Faz com que todas as views sejam aprovadas,
                // Porem elas precisam de "AllowAnonymous" no momento da autenticação para serem carregadas.
                config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(); // Quando tento acessar algo no navegador, ele bate aqui e depois aponta pro endPoint.

            // Aqui são injetadas todas as infos de acesso que o sistema posuir cadastradas.
            // Primeiro pregunta quem é vc.
            app.UseAuthentication();

            // Verifica se o usuário possui acesso, após verificar a rota e antes de acessar o endereço final.
            // Depois Pergunta se vc está aprovado.
            app.UseAuthorization(); // Quando chega aqui ele vai pra configuração la em cima e verifica a autorização e a politica.

            // Ao bater aqui, verifica os controlladores e vê se o que eu solicitei está apontado neles.
            app.UseEndpoints(endpoints =>
            {
                // Possibilita utilizar a rota padrão dos controladores.
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
