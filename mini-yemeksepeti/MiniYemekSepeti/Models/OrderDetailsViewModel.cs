namespace MiniYemekSepeti.Models
{
    public class OrderDetailsViewModel
    {
        // The unique identifier of the order. This is used to identify and track the order.
        public int OrderId { get; set; }

        // The date and time when the order was placed. It shows when the order was made.
        public DateTime OrderDate { get; set; }

        // The current status of the order (e.g., "Pending", "Shipped", "Delivered").
        public string Status { get; set; }

        // The payment status of the order (e.g., "Completed", "Pending", "Failed").
        public string PaymentStatus { get; set; }

        // The total amount for the order (including the cost of items and shipping fee).
        public decimal TotalAmount { get; set; }

        // The first name of the customer who placed the order.
        public string CustomerFirstName { get; set; }

        // The last name of the customer who placed the order.
        public string CustomerLastName { get; set; }

        // The email address of the customer. Used for order-related communications.
        public string CustomerEmail { get; set; }

        // The phone number of the customer. Used for contact in case of delivery or urgent matters.
        public string CustomerPhone { get; set; }

        // The name of the food item ordered (e.g., "Cheese Pizza").
        public string FoodName { get; set; }

        // The quantity of the food item ordered.
        public int ItemQuantity { get; set; }

        // The price of a single unit of the food item.
        public decimal ItemPrice { get; set; }

        // The total amount for the specific food item (ItemPrice * ItemQuantity).
        public decimal ItemTotalAmount { get; set; }
    }
}
