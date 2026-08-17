using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class UsuarioActualService : IUsuarioActualService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioActualService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool EstaAutenticado =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

        public int? IdUsuario
        {
            get
            {
                var valor = _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(valor, out var id) ? id : null;
            }
        }

        public string? Rol =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    }
}