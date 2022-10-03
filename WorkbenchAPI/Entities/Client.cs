using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public bool isActive { get; set; }
        public virtual Role Role { get; set; }
        public int RoleId { get; set; }
        public virtual List<Shop>? Shops { get; set; }
        
    }
}
