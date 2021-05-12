using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace IdentityExample
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Essa e a config abaixo, possuem a mesma configuração que o addSingleton,
            // Só que essas configurações fornecem suas proprias classes.
            // Com essa configuração vc consegue injetar as infos do context em qualquer lugar da aplicação.
            services.AddDbContext<AppDbContext>(config => 
            {
                config.UseInMemoryDatabase("Memory");
            });

            // Registra um conjunto de repositorios/serviços .
            // repositorios = É uma coleção de funções/interfaces que abstraem sua chamadas para o banco de dados.
            // Com esse registro vc possibilita a injeção de dependencia desses itens nos controllers.
            services.AddIdentity<IdentityUser, IdentityRole>(config => 
            {
                // Configurações para remover as configs padrões de password e adicionar as suas.
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = true; // Não deixa o ususário acessar até confirmar o e-mail.
            })
                .AddEntityFrameworkStores<AppDbContext>() // Configura com qual database o identity vai se comunicar.
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });

            // Essa linha configura o envio de email, a 'section' do appsettings, 
            // no mundo real, precisa das infos de segurança do objeto 'MailKitOptions'.
            services.AddMailKit(config => config.UseMailKit(_config.GetSection("Email").Get<MailKitOptions>()));

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