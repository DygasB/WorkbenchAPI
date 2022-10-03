using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Models
{
    public class RegisterClientDto
    {
        //[Required]
        public string? Email { get; set; }
        //[Required]
        //[MinLength(6)]
        public string? ConfirmPassword { get; set; }
        public string? Password { get; set; }
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; } = 1; 
    }
}
