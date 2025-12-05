using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Fiora.Models;
using Fiora.Data;

namespace Fiora.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            string[] roles = { "Admin", "Cliente" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedDefaultAdminAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var db = services.GetRequiredService<ApplicationDbContext>();

            // Default admin credentials from configuration or fallback
            var config = services.GetRequiredService<IConfiguration>();
            var adminEmail = config["SeedAdmin:Email"] ?? "admin@fiora.local";
            var adminPassword = config["SeedAdmin:Password"] ?? "Admin123!";
            var adminName = config["SeedAdmin:Name"] ?? "Administrador";

            var existingUser = await userManager.FindByEmailAsync(adminEmail);
            if (existingUser == null)
            {
                var identityUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var createResult = await userManager.CreateAsync(identityUser, adminPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityUser, "Admin");

                    // Create domain Admin row if not exists
                    var existsAdmin = db.Admin.Any(a => a.CorreoAdmin == adminEmail);
                    if (!existsAdmin)
                    {
                        db.Admin.Add(new Admin
                        {
                            NombreAdmin = adminName,
                            CorreoAdmin = adminEmail,
                            PasswordAdmin = adminPassword,
                            Estado = EstadoAdmin.Activo
                        });
                        await db.SaveChangesAsync();
                    }
                }
            }
        }

        public static async Task SeedDefaultClienteAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var db = services.GetRequiredService<ApplicationDbContext>();
            var config = services.GetRequiredService<IConfiguration>();

            var clienteEmail = config["SeedCliente:Email"] ?? "cliente@fiora.local";
            var clientePassword = config["SeedCliente:Password"] ?? "Cliente123!";
            var clienteNombre = config["SeedCliente:Nombre"] ?? "Cliente Demo";
            var clienteTelefono = config["SeedCliente:Telefono"] ?? "77777777";
            var clienteDireccion = config["SeedCliente:Direccion"] ?? "Dirección Demo";

            var existingUser = await userManager.FindByEmailAsync(clienteEmail);
            if (existingUser == null)
            {
                var identityUser = new IdentityUser { UserName = clienteEmail, Email = clienteEmail, EmailConfirmed = true };
                var createResult = await userManager.CreateAsync(identityUser, clientePassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityUser, "Cliente");

                    var existsCliente = db.Cliente.Any(c => c.CorreoCliente == clienteEmail);
                    if (!existsCliente)
                    {
                        db.Cliente.Add(new Cliente
                        {
                            NombreCliente = clienteNombre,
                            CorreoCliente = clienteEmail,
                            PasswordCliente = clientePassword,
                            TelefonoCliente = clienteTelefono,
                            DireccionCliente = clienteDireccion,
                            Estado = EstadoCliente.Activo,
                            FechaRegistroCliente = DateTime.UtcNow
                        });
                        await db.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
