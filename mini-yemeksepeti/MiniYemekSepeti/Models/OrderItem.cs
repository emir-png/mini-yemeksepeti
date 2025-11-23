using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniYemekSepeti.Models
{
    // The OrderItem class represents a specific item in an order, and it maps to the "OrderItems" table in the database.
    [Table("OrderItems")]
    public class OrderItem
    {
        // The unique identifier for this order item.
        public int Id { get; set; }

        // The quantity of the specific food item in the order.
        public int Quantity { get; set; }

        // The price of a single unit of the food item in the order.
        [Precision(16, 2)]  // Ensures the decimal value can store up to 16 digits, with 2 digits after the decimal point.
        public decimal UnitPrice { get; set; }

        // Navigation property representing the food item associated with this order item.
        // This allows access to the detailed information about the food item (like its name, category, etc.).
        public Food Food { get; set; } = new Food();
    }
}
