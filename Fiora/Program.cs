using Fiora.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Use SQL Server in Production; fallback to SQLite in Development for reliability
if (builder.Environment.IsDevelopment())
{
    var sqliteConn = builder.Configuration.GetConnectionString("DefaultConnectionSqlite")
        ?? $"Data Source={Path.Combine(AppContext.BaseDirectory, "fiora_dev.db")}";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
        .UseSqlite(sqliteConn));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
        .UseSqlServer(connectionString, sql =>
        {
            sql.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));
}
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure post-login redirect path
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ensure database is ready and seed roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    if (app.Environment.IsDevelopment())
    {
        // With SQLite in Development, prefer EnsureCreated to avoid SQL Server-specific migrations
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }
    else
    {
        await db.Database.MigrateAsync();
    }
    await RoleSeeder.SeedRolesAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Show detailed exceptions in Development
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

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

// Do not map Razor Pages to avoid shadowing MVC routes

// Optional: fallback to Home/Error for unmatched routes
app.MapFallbackToController("Error", "Home");

app.Run();
