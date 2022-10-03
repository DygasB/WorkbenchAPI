using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Entities
{
    /// <summary>
    /// Klasa odpowiedzialna za połączenie z bazą danych poprzez DI
    /// </summary>
    public class WorkbenchDbContext : DbContext
    {

        public WorkbenchDbContext(DbContextOptions<WorkbenchDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Shop> Shops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Address>(a =>
            {
                a.Property(x => x.City).IsRequired();
                a.Property(x => x.PostalCode).HasColumnType("varchar(200)");
                a.Property(x => x.PostalCode).IsRequired();
            });

            modelBuilder.Entity<Client>(c =>
            {
                c.Property(x => x.DateOfBirth).HasDefaultValueSql("getutcdate()");

                c.HasOne(x => x.Role)
                .WithMany(r => r.Clients)
                .HasForeignKey(x => x.RoleId);
                    
            });

            modelBuilder.Entity<Product>(p =>
            {
                p.Property(x => x.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Role>(r =>
            {
                r.Property(x => x.Name).HasColumnName("User_name");
                r.Property(x => x.Name).ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<Shop>(s =>
            {
                s.Property(x => x.Name).IsRequired();

                s.HasOne(x => x.Address)
                .WithMany(a => a.Shops)
                .HasForeignKey(x => x.AddressId);

                s.HasMany(x => x.Products)
                 .WithOne(p => p.Shop)
                 .HasForeignKey(p => p.ShopId);

                
                //relacja wiele do wielu w nowszych wersjach dotnet (bez tabeli łączącej)
                s.HasMany(sh => sh.Clients)
                .WithMany(cl => cl.Shops)
                .UsingEntity<ShopClient>(

                    x => x.HasOne(sc => sc.Client)
                    .WithMany()
                    .HasForeignKey(sc => sc.ClientId),

                    x => x.HasOne(sc => sc.Shop)
                    .WithMany()
                    .HasForeignKey(sc => sc.ShopId),

                    sc =>
                    {
                        sc.HasKey(x => new { x.ClientId, x.ShopId });
                        sc.Property(x => x.DateOfJoining).HasDefaultValueSql("getutcdate()");
                    }
                    );
                
            });
            
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        //Metody: HasDefaultValue(), HasPrecision, ValueGeneratedOnUpdate
    }
}
