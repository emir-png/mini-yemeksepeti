using MiniYemekSepeti.Models;
using MiniYemekSepeti.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace MiniYemekSepeti.Controllers
{
    [Authorize(Roles = "admin")] // Only users with the "admin" role can access this controller.
    [Route("/Admin/Orders/{action=Index}/{id?}")] // Defines the routing pattern for this controller.
    public class AdminOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderService _orderService;  // OrderService provides business logic related to orders.
        private readonly string _connectionString;
        // Constructor that initializes the database context and OrderService.
        public AdminOrdersController(ApplicationDbContext context, OrderService orderService, IConfiguration configuration)
        {
            _context = context; // Initialize the database context.
            _orderService = orderService;  // Initialize the OrderService.
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index(int id)
        {
            // Fetch all orders with related client and items from the database.
            var orders = _context.Orders.Include(o => o.Client).Include(o => o.Items).ToList();
            ViewBag.Orders = orders; // Pass the list of orders to the view.

            // Toplam sipariş tutarını alıyoruz
            decimal totalAmount = GetTotalAmountForAllOrders();

            // ViewBag'e toplam sipariş tutarını aktarıyoruz
            ViewBag.TotalAmount = totalAmount;

            // Toplam sipariş sayısını alıyoruz
            int totalOrders = GetTotalOrdersCount();

            // ViewBag'e toplam sipariş sayısını aktarıyoruz
            ViewBag.TotalOrders = totalOrders;

            return View();
        }


        private int GetTotalOrdersCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT dbo.fn_GetTotalOrdersCount()", connection))
                {
                    // Fonksiyonun döndürdüğü değeri alıyoruz
                    return (int)command.ExecuteScalar();
                }
            }
        }


        private decimal GetTotalAmountForAllOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT dbo.fn_GetTotalAmountForAllOrders()", connection))
                {
                    // Fonksiyonun döndürdüğü değeri alıyoruz
                    return (decimal)command.ExecuteScalar();
                }
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the order with related client and items from the database.
            var order = await _context.Orders.Include(o => o.Client)
                                             .Include(o => o.Items)
                                             .ThenInclude(oi => oi.Food)
                                             .FirstOrDefaultAsync(o => o.Id == id);

            // Redirect to the index if the order is not found.
            if (order == null)
            {
                return RedirectToAction("Index");
            }

            // Pass payment and order statuses to the view.
            ViewBag.PaymentStatus = order.PaymentStatus;
            ViewBag.OrderStatus = order.OrderStatus;

            // Get the total number of orders placed by the client.
            ViewBag.NumOrders = _context.Orders.Where(o => o.ClientId == order.ClientId).Count();

            // Get the total amount for the specific order via OrderService.
            var totalAmount = await _orderService.GetTotalAmountAsync(id);
            ViewBag.TotalAmount = totalAmount;  // Pass the total amount of the order to the view.

            // Get the total amount for all orders via OrderService.
            var totalAllOrdersAmount = await _orderService.GetTotalAmountForAllOrdersAsync();
            ViewBag.TotalAllOrdersAmount = totalAllOrdersAmount; // Pass the total amount for all orders to the view.

            return View(order);
        }

        public async Task<IActionResult> Edit(int id, string? payment_status, string? order_status)
        {
            // Fetch the order from the database.
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }

            // If both statuses are null, redirect to the order details page.
            if (order_status == null && payment_status == null)
            {
                return RedirectToAction("Details", new { id });
            }

            try
            {
                // If order status is provided, update the order status via a stored procedure.
                if (order_status != null)
                {
                    _context.Database.ExecuteSqlRaw(
                        "EXEC [dbo].[UpdateOrderStatus] @OrderId = @p0, @NewOrderStatus = @p1", //updateorderst proc
                        id,
                        order_status);
                }

                // If payment status is provided, update the payment status via a stored procedure.
                if (payment_status != null)
                {
                    _context.Database.ExecuteSqlRaw(
                        "EXEC [dbo].[UpdatePaymentStatus] @OrderId = @p0, @NewPaymentStatus = @p1", //update payment st proc
                        id,
                        payment_status);
                }

                // Save changes to the database.
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the update process.
                ViewBag.ErrorMessage = "An error occurred while updating the order: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }

            // After updating, redirect to the order details page.
            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.Include(o => o.Client).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                // Before deleting, create a notification.
                var notification = new Notification
                {
                    UserId = order.ClientId,
                    NotificationMessage = "Your order has been deleted.",
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(notification); // Add the notification to the database.

                // Delete the order.
                _context.Database.ExecuteSqlRaw("EXEC DeleteOrder @p0", id);//do sp 

                // Save changes to the database.
                await _context.SaveChangesAsync();

                // Redirect to the index page after successful deletion.
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the delete process.
                ViewBag.ErrorMessage = "An error occurred while deleting the order: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Logs()
        {
            // Get all order logs from the database in descending order of creation date.
            var logs = await _context.OrderLogs.OrderByDescending(l => l.CreatedAt).ToListAsync();
            return View(logs);
        }

        public async Task<IActionResult> Notifications()
        {
            // Get all notifications from the database in descending order of creation date.
            var notifications = await _context.Notifications.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return View(notifications);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult ClearNotifications()
        {
            // Insert a record in the TriggerControl table to trigger notifications.
            _context.Database.ExecuteSqlRaw("INSERT INTO TriggerControl (TriggerActivated) VALUES (1)");

            // Redirect to the notifications page.
            return RedirectToAction("Notifications");
        }

        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            // Fetch order details from the SQL View based on the order ID.
            var orderDetails = _context.OrderDetailsViews
                .Where(od => od.OrderId == id)
                .ToList();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound(); // Return 404 if no order details found.
            }

            // Pass the order details to the view.
            return View(orderDetails);
        }
    }
}





 