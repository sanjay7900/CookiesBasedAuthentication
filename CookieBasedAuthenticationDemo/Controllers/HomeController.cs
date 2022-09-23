using CookieBasedAuthenticationDemo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CookieBasedAuthenticationDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Admin()
        {
            return View();
        }
        [Authorize(Roles ="User")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public IActionResult User()
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Verify(string username, string password)
        {
            string returnurl = "Index";
            if (username=="Admin" && password=="Admin")
            {
                AddClaims(username, "Admin");
                return RedirectToAction(returnurl);
            }
            else if (username=="User" && username == "User")
            {
                AddClaims(username, "User");
                return RedirectToAction(returnurl);

            }
            else
            {
                return BadRequest();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private void AddClaims(string name,string role)
        {
            List<Claim> climList = new List<Claim>();
            climList.Add(new Claim(ClaimTypes.Name,name));
            climList.Add(new Claim(ClaimTypes.Role,role));
            ClaimsIdentity  claims=new ClaimsIdentity(climList,CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal= new ClaimsPrincipal(claims);
            HttpContext.SignInAsync(claimsPrincipal);


        }
        public IActionResult LogOff()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index");
        }
    }
}