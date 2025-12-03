using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fiora.Models;

namespace Fiora.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fiora.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<Fiora.Models.Admin> Admin { get; set; } = default!;
        public DbSet<Fiora.Models.Arreglo> Arreglo { get; set; } = default!;
        public DbSet<Fiora.Models.Pedido> Pedido { get; set; } = default!;
        public DbSet<Fiora.Models.Inventario> Inventario { get; set; } = default!;
        public DbSet<Fiora.Models.ArregloInventario> ArregloInventario { get; set; } = default!;
        public DbSet<Fiora.Models.PedidoInventario> PedidoInventario { get; set; } = default!;
        public DbSet<Fiora.Models.Reporte> Reporte { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure decimal precision to avoid truncation warnings
            builder.Entity<Fiora.Models.Arreglo>()
                .Property(a => a.PrecioArreglo)
                .HasPrecision(18, 2);

            builder.Entity<Fiora.Models.Pedido>()
                .Property(p => p.MontoTotal)
                .HasPrecision(18, 2);

            builder.Entity<Fiora.Models.PedidoInventario>()
                .Property(pi => pi.PrecioUnitario)
                .HasPrecision(18, 2);
        }
    }
}
