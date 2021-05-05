using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

            // Para poder chamar as views, a parte de front-end apartir dos controllers.
            // Só declarar o controller com a Herança Controller e não ControllerBase.
            services.AddControllersWithViews();
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
            app.UseAuthorization(); 

            // Ao bater aqui, verifica os controlladores e vê se o que eu solicitei está apontado neles.
            app.UseEndpoints(endpoints =>
            {
                // Possibilita utilizar a rota padrão dos controladores.
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
