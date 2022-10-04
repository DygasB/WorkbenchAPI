using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Models
{
    public class ShopQuery
    {
        public string? SearchPhrase { get; set; } 
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
