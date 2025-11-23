using System.ComponentModel.DataAnnotations;

namespace MiniYemekSepeti.Models
{
    // The FoodDto class represents a data transfer object used for transferring food data.
    // It includes properties like Name, Restaurant, Category, Price, Description, and an optional image file.
    public class FoodDto
    {
        // Represents the name of the food item. This field is required and has a maximum length of 100 characters.
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        // Represents the restaurant the food item belongs to. This field is required and has a maximum length of 100 characters.
        [Required, MaxLength(100)]
        public string Restaurant { get; set; } = "";

        // Represents the category of the food item (e.g., "Pizza", "Dessert"). This field is required and has a maximum length of 100 characters.
        [Required, MaxLength(100)]
        public string Category { get; set; } = "";

        // Represents the price of the food item. This field is required.
        [Required]
        public decimal Price { get; set; }

        // Represents a description of the food item. This field is required.
        [Required]
        public string Description { get; set; } = "";

        // Represents the image file associated with the food item. This is an optional field that can be null.
        // The IFormFile interface is typically used to handle file uploads in ASP.NET Core.
        public IFormFile? ImageFile { get; set; }
    }
}
