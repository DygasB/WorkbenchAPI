using System.Security.Claims;

namespace WorkbenchAPI.Services
{
    public interface IClientContextService
    {
        ClaimsPrincipal Client { get; }
        int? GetUserId { get; }
    }
}