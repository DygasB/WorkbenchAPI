using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Entities
{
    public class ShopClient
    {
        public Shop Shop { get; set; }  
        public int ShopId { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public DateTime DateOfJoining { get; set; }
    }
}
