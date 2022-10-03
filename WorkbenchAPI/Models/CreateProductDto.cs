using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;

namespace WorkbenchAPI.Models
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Type { get; set; }
        public int ShopId { get; set; }
    }
}
