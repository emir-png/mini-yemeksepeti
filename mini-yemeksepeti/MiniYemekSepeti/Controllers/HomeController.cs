using MiniYemekSepeti.Models; // Using the MiniYemekSepeti.Models namespace, which contains the application's model classes.
using Microsoft.AspNetCore.Mvc; // Using the Microsoft.AspNetCore.Mvc namespace, which contains necessary classes for the MVC framework.
using System.Linq;
using MiniYemekSepeti.Services; // Using the LINQ query namespace for database queries.

namespace MiniYemekSepeti.Controllers // Defining the MiniYemekSepeti.Controllers namespace, which contains controller classes.
{
    public class HomeController : Controller // HomeController is a class that inherits from Controller. It handles requests from the user.
    {
        private readonly ApplicationDbContext context; // Field to store the database context (ApplicationDbContext).

        // Constructor for HomeController, it takes ApplicationDbContext as a parameter and initializes the context.
        public HomeController(ApplicationDbContext context)
        {
            this.context = context; // Assigning the passed ApplicationDbContext to the class field.
        }

        // This method loads the main page and retrieves the last 4 products from the database.
        public IActionResult Index()
        {
            var foods = context.Food.OrderByDescending(p => p.Id).Take(4).ToList(); // Fetching the last 4 products ordered by their Id in descending order.
            return View(foods); // Sending the retrieved products to the View.
        }

        // This method handles the error page and is called in case of an error.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Specifies that the response should not be cached.
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier }); // Returns an Error View with an error model containing the TraceIdentifier.
        }
    }
}
