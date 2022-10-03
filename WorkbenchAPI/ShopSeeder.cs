using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;

namespace WorkbenchAPI
{
    public class ShopSeeder
    {
        private readonly WorkbenchDbContext dbContext;

        public ShopSeeder(WorkbenchDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Seed()
        {
            if (dbContext.Database.CanConnect())
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations is not null && pendingMigrations.Any())
                {
                    dbContext.Database.Migrate();
                }
                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    dbContext.SaveChanges();
                }
                if (!dbContext.Shops.Any())
                {
                    var shops = GetShops();
                    dbContext.Shops.AddRange(shops);
                    dbContext.SaveChanges();
                }
            }
            
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Employee",
                },
                new Role()
                {
                    Name = "Leader"
                },
                new Role()
                {
                    Name = "Boss"
                }
            };
            return roles;
        }
        private IEnumerable<Shop> GetShops()
        {
            var shops = new List<Shop>()
            {
                new Shop()
                {
                    Name = "Sklep ogrodowy",
                    Description = "Sprzedaż hurtowa i detaliczna akcesoriow ogrodowych",
                    Category = "Ogrod",
                    HasDelivery = true,
                    ContactEmail= "sklepogrodowy@gmail.com",
                    Address = new Address()
                    {
                        City="Katowice",
                        Street="Słoneczna 5",
                        PostalCode="40-000"
                    },
                    Products=new List<Product>()
                    {
                        new Product()
                        {
                            Name="Kwiat",
                            Type="Rośliny"
                        },
                        new Product()
                        {
                            Name="Nasiona dyni",
                            Type="Nasiona"
                        }
                    },


                }
            };
            return shops;
        }
    }
}
