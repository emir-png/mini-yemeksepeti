using MiniYemekSepeti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniYemekSepeti.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MiniYemekSepeti.Controllers
{
    // Only authorized users with the "client" role can access this controller
    [Authorize(Roles = "client")]
    [Route("/Client/Orders/{action=Index}/{id?}")]
    public class ClientOrdersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly string _connectionString;

        // Constructor to initialize context, user manager, and connection string
        public ClientOrdersController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Retrieves and displays all orders for the current client user
        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch all orders for the client, along with their items and associated food details
            var orders = await context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Food)
                .Where(o => o.ClientId == currentUser.Id)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            ViewBag.Orders = orders;

            return View();
        }

        // Displays detailed information for a specific order
        public async Task<IActionResult> Details(int id)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch the specific order with items and food details
            var order = await context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Food)
                .FirstOrDefaultAsync(o => o.ClientId == currentUser.Id && o.Id == id);

            if (order == null)
            {
                return RedirectToAction("Index");
            }

            // Calculate the total amount using SQL Server function
            decimal totalAmount = await GetTotalAmountFromDatabase(id);

            // Pass the total amount to the view
            ViewData["TotalAmount"] = totalAmount;

            return View(order);
        }

        // Helper method to call the SQL Server function and get the total amount
        private async Task<decimal> GetTotalAmountFromDatabase(int orderId)
        {
            decimal totalAmount = 0;

            // Use ADO.NET to connect to the database and call the function
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT [dbo].[CalculateTotalAmount](@OrderId)", connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    // Execute the SQL command and retrieve the result
                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        totalAmount = Convert.ToDecimal(result);
                    }
                }
            }

            return totalAmount;
        }

        // Displays notifications for the current client user
        public async Task<IActionResult> Notifications()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch notifications for the client user, ordered by their creation date
            var notifications = await context.Notifications
                .Where(n => n.UserId == currentUser.Id)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            // Mark all unread notifications as read
            foreach (var notification in notifications.Where(n => !n.IsRead))
            {
                notification.IsRead = true;
            }

            await context.SaveChangesAsync(); // Save changes to the database

            return View(notifications); // Return the notifications view to display them
        }

        // This method executes before any action in the controller to check unread notifications count
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Retrieve the current user's ID from claims
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                // Count unread notifications for the current user
                var unreadNotificationsCount = this.context.Notifications
                    .Where(n => n.UserId == currentUserId && !n.IsRead)
                    .Count();

                // Pass the unread notifications count to the view
                ViewBag.UnreadNotificationsCount = unreadNotificationsCount;
            }
        }

        // Marks all notifications as cleared and triggers an event to update the database
        [HttpPost]
        [Authorize(Roles = "client")]
        public IActionResult ClearNotifications()
        {
            // Insert a record in the TriggerControl table to activate some kind of trigger event
            context.Database.ExecuteSqlRaw("INSERT INTO TriggerControl (TriggerActivated) VALUES (1)");

            // Redirect to the notifications page
            return RedirectToAction("Notifications");
        }
    }
}
