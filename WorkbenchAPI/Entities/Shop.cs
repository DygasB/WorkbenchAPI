using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Entities
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; }
        public virtual Address Address { get; set; }
        public int AddressId { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();
        public virtual List<Client> Clients { get; set; } = new List<Client>();
    }
}
