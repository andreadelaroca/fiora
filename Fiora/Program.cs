using Fiora.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Always use SQLite for local development, single-file DB
var sqliteConn = builder.Configuration.GetConnectionString("DefaultConnectionSqlite")
    ?? $"Data Source={Path.Combine(AppContext.BaseDirectory, "fiora_dev.db")}";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
    .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
    .UseSqlite(sqliteConn));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure post-login redirect path
builder.Services.ConfigureApplicationCookie(options =>
{
    // Use the default Identity UI Razor Pages paths
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ensure database is ready and seed roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    // For SQLite, recreate schema to keep it in sync easily
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
    await RoleSeeder.SeedRolesAsync(services);
    await RoleSeeder.SeedDefaultAdminAsync(services);
}

// Configure the HTTP request pipeline.
// Show detailed exceptions always in this local setup
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseRouting();

// Enable authentication before authorization
app.UseAuthentication();

// Re-execute pipeline on 404 to error handler
app.UseStatusCodePagesWithReExecute("/Home/Error");

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Map Razor Pages so Identity UI (login, register) works
app.MapRazorPages();

// Legacy route redirects for convenience
app.MapGet("/Account/Login", () => Results.Redirect("/Identity/Account/Login"));
app.MapGet("/Account/Register", () => Results.Redirect("/Identity/Account/Register"));
app.MapGet("/Account/AccessDenied", () => Results.Redirect("/Identity/Account/AccessDenied"));

// Optional: fallback to Home/Error for unmatched routes
app.MapFallbackToController("Error", "Home");

app.Run();
