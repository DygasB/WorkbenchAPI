using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Services
{
    public class ClientContextService : IClientContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal Client => _httpContextAccessor.HttpContext?.User;
        public int? GetUserId => Client is null ? null : (int?)int.Parse(Client.FindFirst(
            c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
