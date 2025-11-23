using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniYemekSepeti.Models;
using MiniYemekSepeti.Services;

namespace MiniYemekSepeti.Controllers
{
    // Restrict access to users with the 'admin' role
    [Authorize(Roles = "admin")]
    [Route("/Admin/[controller]/{action=Index}/{id?}")]
    public class FoodController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly int pageSize = 5;  // Pagination: number of items per page

        // Constructor to inject dependencies (DbContext and IWebHostEnvironment)
        public FoodController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        // Action method to display the list of food items with pagination and search functionality
        public IActionResult Index(int pageIndex, string? search)
        {
            IQueryable<Food> query = context.Food;

            // Search functionality: filters the list based on food name, category, or restaurant
            if (search != null)
            {
                query = query.Where(p => p.Name.Contains(search) || p.Category.Contains(search) || p.Restaurant.Contains(search));
            }

            // Order the food items by descending order of their Id
            query = query.OrderByDescending(p => p.Id);

            // Ensure the pageIndex is at least 1
            if (pageIndex < 1)
            { pageIndex = 1; }

            // Get the total count of food items and calculate total pages for pagination
            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);

            // Apply pagination logic (skip and take)
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            // Execute the query and get the list of food items
            var food = query.ToList();

            // Pass pagination data to the view
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;
            ViewData["Search"] = search ?? "";

            return View(food);
        }

        // Action method to show the food creation form
        public IActionResult Create()
        {
            return View();
        }

        // POST method to handle food creation (adding a new food item)
        [HttpPost]
        public async Task<IActionResult> Create(FoodDto foodDto)
        {
            // Check if the image file is provided, otherwise add a model state error
            if (foodDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            // If model state is invalid, return the view with the current foodDto to show validation errors
            if (!ModelState.IsValid)
            {
                return View(foodDto);
            }

            // Generate a unique filename for the uploaded image based on the current time
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(foodDto.ImageFile!.FileName);
            string imageFullPath = Path.Combine(environment.WebRootPath, "products", newFileName);

            // Save the image file to the "products" folder
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                await foodDto.ImageFile.CopyToAsync(stream);
            }

            // Create parameters for the stored procedure to add a food item
            var nameParam = new SqlParameter("@Name", foodDto.Name);
            var restaurantParam = new SqlParameter("@Restaurant", foodDto.Restaurant);
            var categoryParam = new SqlParameter("@Category", foodDto.Category);
            var priceParam = new SqlParameter("@Price", foodDto.Price);
            var descriptionParam = new SqlParameter("@Description", foodDto.Description);
            var imageFileNameParam = new SqlParameter("@ImageFileName", newFileName);

            // Call the stored procedure to add the food item to the database
            await context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.AddFood @Name, @Restaurant, @Category, @Price, @Description, @ImageFileName",
                nameParam, restaurantParam, categoryParam, priceParam, descriptionParam, imageFileNameParam);

            // Redirect to the food list page after successful creation
            return RedirectToAction("Index", "Food");
        }

        // Action method to show the form to edit an existing food item
        public IActionResult Edit(int id)
        {
            var food = context.Food.Find(id);

            // If the food item doesn't exist, redirect to the food list page
            if (food == null)
            {
                return RedirectToAction("Index", "Food");
            }

            // Prepare a FoodDto with existing food data for editing
            var foodDto = new FoodDto()
            {
                Name = food.Name,
                Restaurant = food.Restaurant,
                Category = food.Category,
                Price = food.Price,
                Description = food.Description,
            };

            // Pass existing food data to the view for editing
            ViewData["FoodId"] = food.Id;
            ViewData["ImageFileName"] = food.ImageFileName;

            return View(foodDto);
        }

        // POST method to handle the food item update
        [HttpPost]
        public IActionResult Edit(int id, FoodDto foodDto)
        {
            var food = context.Food.Find(id);

            // If the food item doesn't exist, redirect to the food list page
            if (food == null)
            {
                return RedirectToAction("Index", "Food");
            }

            // If model state is invalid, return the view with validation errors
            if (!ModelState.IsValid)
            {
                ViewData["FoodId"] = food.Id;
                ViewData["ImageFileName"] = food.ImageFileName;
                return View(foodDto);
            }

            // Update the image file if a new image is provided
            string newFileName = food.ImageFileName;
            if (foodDto.ImageFile != null)
            {
                // Generate a new filename for the new image
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(foodDto.ImageFile.FileName);

                // Save the new image to the "products" folder
                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    foodDto.ImageFile.CopyTo(stream);
                }

                // Delete the old image if it exists
                string oldImageFullPath = environment.WebRootPath + "/products/" + food.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            // Update the food item in the database with new values
            food.Name = foodDto.Name;
            food.Restaurant = foodDto.Restaurant;
            food.Category = foodDto.Category;
            food.Price = foodDto.Price;
            food.Description = foodDto.Description;
            food.ImageFileName = newFileName;

            // Save the changes to the database
            context.SaveChanges();

            // Redirect to the food list page after successful update
            return RedirectToAction("Index", "Food");
        }

        // Action method to delete a food item
        public IActionResult Delete(int id)
        {
            var food = context.Food.Find(id);

            // If the food item doesn't exist, redirect to the food list page
            if (food == null)
            {
                return RedirectToAction("Index", "Food");
            }

            // Delete the food image from the file system
            string imageFullPath = environment.WebRootPath + "/products/" + food.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            // Remove the food item from the database and save the changes
            context.Food.Remove(food);
            context.SaveChanges(true);

            // Redirect to the food list page after successful deletion
            return RedirectToAction("Index", "Food");
        }
    }
}
