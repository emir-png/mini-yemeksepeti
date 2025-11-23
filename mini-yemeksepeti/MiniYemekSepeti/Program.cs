using MiniYemekSepeti.Models;
using MiniYemekSepeti.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add ApplicationDbContext to the DI container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	// Get the connection string from configuration (appsettings.json or environment variable)
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	// Use SQL Server with the connection string
	options.UseSqlServer(connectionString);
});

// Add Identity services to the DI container for user and role management
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	// Password policy configuration
	options.Password.RequiredLength = 6; // Minimum length of 6 characters
	options.Password.RequireNonAlphanumeric = false; // No special character required
	options.Password.RequireUppercase = false; // No uppercase letter required
	options.Password.RequireLowercase = false; // No lowercase letter required
})
	.AddEntityFrameworkStores<ApplicationDbContext>(); // Use the ApplicationDbContext to store Identity data

// Add the OrderService to the DI container for order-related business logic
builder.Services.AddScoped<OrderService>(); // Scoped lifetime, creates a new instance per request

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	// In production environment, use exception handling page
	app.UseExceptionHandler("/Home/Error");
	// Use HTTP Strict Transport Security (HSTS)
	app.UseHsts();
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles(); // Serve static files (CSS, JS, images)
app.UseRouting(); // Enable routing for requests
app.UseAuthorization(); // Enable authorization for users

// Default route configuration
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"); // Default controller and action if not specified

// Create and seed initial data (roles and admin user) if not already present
using (var scope = app.Services.CreateScope())
{
	var userManager = scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>))
		as UserManager<ApplicationUser>;
	var roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))
		as RoleManager<IdentityRole>;

	// Call the method to seed roles and admin user
	await DatabaseInitializer.SeedDataAsync(userManager, roleManager);
}

// Run the application
app.Run();
