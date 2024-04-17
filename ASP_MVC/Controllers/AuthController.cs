using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ASP_MVC.Data.Models;
using ASP_MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC.Controllers
{
    public class AuthController : Controller
    {

        private readonly MydbContext _context;

        public AuthController(MydbContext mydbContext)
        {
            _context = mydbContext;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login(string login, string password)
        {

            _context.Roles.ToList();
            Staff? person = await _context.Staff.FirstOrDefaultAsync(p => p.Login == login && p.Password == password);

            if (person is null) return RedirectToAction("Index", "Home");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, person.Fullname),
                new Claim(ClaimTypes.Role, person.RoleNavigation.RoleName),
                
                // Добавьте другие необходимые права доступа пользователя
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // настройте срок действия аутентификации
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        

    }
}
