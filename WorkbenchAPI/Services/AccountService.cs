using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Exceptions;
using WorkbenchAPI.Models;

namespace WorkbenchAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly WorkbenchDbContext _workbenchDbContext;
        private readonly IPasswordHasher<Client> _passwordHasher;
        private readonly AuthenticationSettings _authentication;
        public AccountService(WorkbenchDbContext workbenchDbContext, IPasswordHasher<Client> passwordHasher, AuthenticationSettings authentication )
        {
            _workbenchDbContext = workbenchDbContext;
            _passwordHasher = passwordHasher;
            _authentication=authentication;
        }
        public void RegisterUser(RegisterClientDto dto)
        {

            var newClient = new Client()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newClient, dto.Password);
            newClient.PasswordHash = hashedPassword;

            _workbenchDbContext.Clients.Add(newClient);
            _workbenchDbContext.SaveChanges();
        }
        public string GenerateJwt(LoginDto dto)
        {
            var client = _workbenchDbContext.Clients
                .Include(c=>c.Role)
                .FirstOrDefault(x => x.Email == dto.Email);
            if (client is null)
                throw new BadRequestException("Invalid username or password");
            var result = _passwordHasher.VerifyHashedPassword(client, client.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid username or password");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(ClaimTypes.Role, $"{client.Role.Name}"),
                new Claim("DateOfBirth", client.DateOfBirth.Value.ToString("yyyy-MM-dd")),
                //new Claim("Nationality" , client.Nationality)
            };
            if (!string.IsNullOrEmpty(client.Nationality))
            {
                claims.Add(
                    new Claim("Nationality", client.Nationality)
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authentication.JwtKey));
            //kredencjaly potrzebne do podpisania tokenu jwt
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //data waznosci tokenu
            var expires = DateTime.Now.AddDays(_authentication.JwtExpireDays);
            //utworzenie tokenu
            var token = new JwtSecurityToken(
                _authentication.JwtIssuer,
                _authentication.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
