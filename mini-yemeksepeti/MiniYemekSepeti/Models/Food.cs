using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MiniYemekSepeti.Models
{
    // The Food class represents an individual food item in the application.
    // It contains properties like Name, Restaurant, Category, Price, Description, and Image for each food item.
    public class Food
    {
        // The unique identifier for the food item. This is typically the primary key in the database.
        public int Id { get; set; }

        // The name of the food item. It is limited to a maximum length of 100 characters.
        [MaxLength(100)]
        public string Name { get; set; } = "";

        // The restaurant associated with the food item. It is also limited to a maximum length of 100 characters.
        [MaxLength(100)]
        public string Restaurant { get; set; } = "";

        // The category to which the food item belongs (e.g., pizza, dessert, etc.).
        // It is limited to a maximum length of 100 characters.
        [MaxLength(100)]
        public string Category { get; set; } = "";

        // The price of the food item. The precision attribute is used to specify that the price can have up to 16 digits in total
        // with 2 digits after the decimal point. This ensures proper formatting for currency values.
        [Precision(16, 2)]
        public decimal Price { get; set; }

        // A description of the food item. There is no explicit length constraint applied here, so it can be any length.
        public string Description { get; set; } = "";

        // The name of the image file associated with the food item. This field has a maximum length of 100 characters.
        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
    }
}
