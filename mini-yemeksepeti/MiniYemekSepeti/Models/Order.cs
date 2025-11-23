using Microsoft.EntityFrameworkCore;

namespace MiniYemekSepeti.Models
{
    // The Order class represents a customer order in the system.
    // It includes details like the client, order items, shipping fees, payment status, and the order status.
    public class Order
    {
        // The unique identifier for the order. This is typically the primary key in the database.
        public int Id { get; set; }

        // The ID of the client who placed the order. This is a foreign key reference to the ApplicationUser entity.
        public string ClientId { get; set; } = "";

        // The actual client object representing the user who placed the order. This is a navigation property.
        public ApplicationUser Client { get; set; } = null!;

        // A list of items in the order. This represents the individual food items or products ordered by the client.
        // This is a one-to-many relationship between Order and OrderItem.
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        // The shipping fee for the order. The Precision attribute specifies that the price can have up to 16 digits in total,
        // with 2 digits after the decimal point to handle currency formatting.
        [Precision(16, 2)]
        public decimal ShippingFee { get; set; }

        // The delivery address where the order will be delivered. This field holds the full address.
        public string DeliveryAddress { get; set; } = "";

        // The payment method chosen for the order (e.g., "Credit Card", "PayPal", "Cash on Delivery").
        public string PaymentMethod { get; set; } = "";

        // The status of the payment for the order (e.g., "Completed", "Pending", "Failed").
        public string PaymentStatus { get; set; } = "";

        // A field to store additional payment details, such as information from PayPal or another payment provider.
       

        // The status of the order (e.g., "Pending", "Completed", "Shipped").
        public string OrderStatus { get; set; } = "";

        // The date and time when the order was created. This can be used to sort orders and track order history.
        public DateTime CreatedAt { get; set; }
    }
}
