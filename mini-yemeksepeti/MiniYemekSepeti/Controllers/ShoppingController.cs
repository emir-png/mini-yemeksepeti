using MiniYemekSepeti.Models;
using MiniYemekSepeti.Services;
using Microsoft.AspNetCore.Mvc;

namespace MiniYemekSepeti.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly ApplicationDbContext context; // The database context to access the Food table
        private readonly int pageSize = 8; // Defines how many items are displayed per page for pagination

        // Constructor to initialize the controller and inject the ApplicationDbContext
        public ShoppingController(ApplicationDbContext context)
        {
            this.context = context; // Assign the injected context to the class field
        }

        // Index action handles displaying a list of food items with search, filtering, sorting, and pagination.
        public IActionResult Index(int pageIndex, string? search, string? restaurant, string? category, string? sort)
        {
            IQueryable<Food> query = context.Food; // Start with all food items from the database

            // Search functionality: Filters food items by name if the 'search' parameter is provided
            if (search != null && search.Length > 0)
            {
                query = query.Where(p => p.Name.Contains(search)); // Filter by food name
            }

            // Filter by restaurant: Filters food items based on the restaurant if the 'restaurant' parameter is provided
            if (restaurant != null && restaurant.Length > 0)
            {
                query = query.Where(p => p.Restaurant.Contains(restaurant)); // Filter by restaurant name
            }

            // Filter by category: Filters food items based on category if the 'category' parameter is provided
            if (category != null && category.Length > 0)
            {
                query = query.Where(p => p.Category.Contains(category)); // Filter by food category
            }

            // Sorting functionality: Sorts food items based on the 'sort' parameter
            if (sort == "price_asc") // Sort by price in ascending order
            {
                query = query.OrderBy(p => p.Price); // Order by price in ascending order
            }
            else if (sort == "price_desc") // Sort by price in descending order
            {
                query = query.OrderByDescending(p => p.Price); // Order by price in descending order
            }
            else // Default sorting by ID in descending order
            {
                query = query.OrderByDescending(p => p.Id); // Order by food ID in descending order
            }

            // Pagination logic: Ensure the 'pageIndex' is at least 1, as pages cannot be less than 1
            if (pageIndex < 1)
            {
                pageIndex = 1; // Default to the first page if pageIndex is less than 1
            }

            // Calculate the total count of food items and determine the total number of pages
            decimal count = query.Count(); // Count the total number of food items after filtering
            int totalPages = (int)Math.Ceiling(count / pageSize); // Calculate total pages for pagination

            // Apply pagination: Skip items from previous pages and take items for the current page
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var food = query.ToList(); // Get the food items for the current page

            // Pass the data to the View via ViewBag
            ViewBag.Food = food; // Pass the list of food items to the view
            ViewBag.PageSize = pageSize; // Pass the page size to the view (how many items per page)
            ViewBag.TotalPages = totalPages; // Pass the total number of pages to the view

            // Create a ShoppingSearchModel to hold the search/filter parameters and pass it to the view
            var shoppingSearchModel = new ShoppingSearchModel()
            {
                Search = search, // Store the current search keyword
                Restaurant = restaurant, // Store the selected restaurant filter
                Category = category, // Store the selected category filter
                Sort = sort // Store the selected sorting option
            };

            return View(shoppingSearchModel); // Return the view with the shopping search model
        }

        // Details action handles displaying a detailed view of a single food item based on the food ID.
        public IActionResult Details(int id)
        {
            var food = context.Food.Find(id); // Find the food item by its ID

            // If the food item is not found, redirect to the shopping index page
            if (food == null)
            {
                return RedirectToAction("Index", "Shopping"); // Redirect to the shopping page if the food is not found
            }

            return View(food); // Return the food details view with the found food item
        }
    }
}
