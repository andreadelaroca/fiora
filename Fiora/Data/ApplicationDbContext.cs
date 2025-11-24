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
    }
}
