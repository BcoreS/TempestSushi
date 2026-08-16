using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Web.Models;

namespace TempestSushi.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IServiceAutenticacion _serviceAutenticacion;

        public AccountController(
            IServiceAutenticacion serviceAutenticacion)
        {
            _serviceAutenticacion = serviceAutenticacion;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _serviceAutenticacion
                .AutenticarAsync(
                    model.Correo,
                    model.Password);

            if (usuario == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Correo electrónico o contraseña incorrectos.");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.IdUsuario.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    usuario.NombreCompleto),

                new Claim(
                    ClaimTypes.Email,
                    usuario.Correo),

                new Claim(
                    ClaimTypes.Role,
                    usuario.Rol)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.Recordarme
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction(
                "Index",
                "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(
                "Login",
                "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}