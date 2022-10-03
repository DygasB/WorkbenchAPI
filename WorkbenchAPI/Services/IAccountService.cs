using WorkbenchAPI.Models;

namespace WorkbenchAPI.Services
{
    public interface IAccountService
    {
        string GenerateJwt(LoginDto dto);
        void RegisterUser(RegisterClientDto dto);
    }
}