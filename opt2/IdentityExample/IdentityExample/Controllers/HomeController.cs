using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
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

        public IActionResult Login(string username, string password)
        {
            // Funcionalidde do login vai aqui
            return RedirectToAction("Index");   
        } 

        public IActionResult Register()
        {

            return View();
        }

        public IActionResult Register(string username, string password)
        {
            // Funcionalidde de registar vai aqui
            return RedirectToAction("Index");
        }
    }
}