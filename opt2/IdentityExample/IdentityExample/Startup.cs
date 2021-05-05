using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityExample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Com essa configuração vc consegue injetar as infos do context em qualquer lugar da aplicação.
            services.AddDbContext<AppDbContext>(config => {
                config.UseInMemoryDatabase("Memory");
            });

            // Registra um conjunto de repositorios/serviços .
            // repositorios = É uma coleção de funções/interfaces que abstraem sua chamadas para o banco de dados.
            // Com esse registro vc possibilita a injeção de dependencia desses itens nos controllers.
            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>() // Configura com qual database o identity vai se comunicar.
                .AddDefaultTokenProviders();

            // services.AddAuthentication("CookieAuth")
            //     .AddCookie("CookieAuth", config =>
            //     {
            //         // Como não tenho esse cookie criado, ele permite a ação ser axecutada, 
            //         // pois não tem nada aprovando ou recusando tal autorização.
            //         config.Cookie.Name = "Grandmas.Cookie";
            //         // Redireciona sempre que tiver a tag 'authorize' e precisa de aprovação.
            //         config.LoginPath = "/Home/Authenticate";
            //     });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}