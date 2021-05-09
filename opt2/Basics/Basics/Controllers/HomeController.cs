using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult Authenticate()
        {
            // Coletania para utilizar na memoria com as "condições" para a autenticação do usuário.
            // Se esse objeto possui vc como informação, logo vc está authorizado.
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob@fmail.com"),
                new Claim(ClaimTypes.DateOfBirth, "11/11/2000"), // Modelo de autenticação amis novo, é oq eu fiz baseado na data.
                new Claim(ClaimTypes.Role, "Admin"), // Role é algo antigo, o atual é usar claim, com uma condição especifica do banco.
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
    }
}