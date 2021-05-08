using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        // Essa configuração e declaração no construtor 
        // seve para vc ter um objeto que manipula as informações dos usuários.
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize] // Usado para proteger um methodo.
        public IActionResult Secret()
        {
            return View();   
        }   

        public IActionResult Login()
        {

            return View();   
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Funcionalidde do login vai aqui
            var user = await _userManager.FindByNameAsync(username);

            if(user != null)
            {
                // processo de login
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if(signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");   
        } 

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            // Funcionalidde de registar vai aqui

            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
            };

            var result = await _userManager.CreateAsync(user, password);

            if(result.Succeeded)
            {
                // gerar o token do email.
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());

                await _emailService.SendAsync("teste@test.com", "email verify", $"<a href=\"{link}\">Verify Email</a>", true);

                return RedirectToAction("EmailVerification");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null) return BadRequest(); // Se for colcoar msg não especifique, apenas diga que algo deu errado.

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View();    
            }

            return BadRequest();
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }
    }
}