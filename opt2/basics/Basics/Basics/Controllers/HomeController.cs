using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Basics.CustomPolicyProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        // private readonly IAuthorizationService _authorizationService;

        // public HomeController(IAuthorizationService authorizationService)
        // {
        //     _authorizationService = authorizationService;
        // }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize] // Usado para proteger um methodo.
        public IActionResult Secret()
        {
            return View();   
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");   
        }   

        [Authorize(Roles = "Admin")] 
        public IActionResult SecretRole()
        {
            return View("Secret");   
        } 

        [SecurityLevel(5)] 
        public IActionResult SecretLevel()
        {
            return View("Secret");   
        }

        [SecurityLevel(10)] 
        public IActionResult SecretHigherLevel()
        {
            return View("Secret");   
        }

        // [AllowAnonymous]
        public IActionResult Authenticate()
        {
            // Coletania para utilizar na memoria com as "condições" para a autenticação do usuário.
            // Se esse objeto possui vc como informação, logo vc está authorizado.
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob@fmail.com"),
                new Claim(ClaimTypes.DateOfBirth, "11/11/2000"), // Modelo de autenticação mais novo, é o que fiz baseado na data.
                new Claim(ClaimTypes.Role, "Admin"), // Role é algo antigo, o atual é usar claim, com uma condição especifica do banco.
                new Claim(DynamicPolicies.SecurityLevel, "7"), // Authorização baseada em um provedor de authorização customizado.
                new Claim("Grandma.Says", "Very nice boy."), // Quando vc quiser uma claim unica, faça assim.
            };

            // Segundo objeto de informações de autenticação, só para exemplo.
            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob K Foo"),
                new Claim("DrivingLicense", "A+"),   
            };

            // inicializa um objeto de identidade de uma autorização, baseado em um obejto contendo os detalhes dessa autorização.
            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            // Conjunto com todas as autorizações registaradas. 
            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            // Define o 'schema' onde estão as permissões dos usuários.
            // Esse é o processo que acontece no background de um sistema de autenticação.
            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");   
        }

        public async Task<IActionResult> DoStuff(
            [FromServices] IAuthorizationService authorizationService)
        {
            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();

            var authResult = await authorizationService.AuthorizeAsync(User, customPolicy);

            if(authResult.Succeeded)
            {
                return View("Index");
            }

            return View("Index");
        }
    }
}