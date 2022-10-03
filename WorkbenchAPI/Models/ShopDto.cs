using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Models
{
    public class ShopDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        [Required]
        [MaxLength(100)]
        public string ContactEmail { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }
        [Required]
        [MaxLength(100)]
        public string PostalCode { get; set; }
        public List<ProductDto> Products { get; set; }
        //[RegularExpression(pattern)], [Phone], [EmailAddress]
    }
}
