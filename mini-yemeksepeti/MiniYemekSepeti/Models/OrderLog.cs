

    namespace MiniYemekSepeti.Models
    {
        public class OrderLog
        {
            // Unique identifier for the log entry
            public int Id { get; set; }

            // The ID of the order this log entry is associated with
            public int OrderId { get; set; }

            // The client (customer) who placed the order
            public string ClientId { get; set; }

            // Timestamp of when the log entry was created
            public DateTime CreatedAt { get; set; }

            // Message describing the event or action taken on the order
            public string LogMessage { get; set; }
        }
    }

    // Purpose of the OrderLog class:
    // 1. **Order Tracking**: Logs each step of the order process (e.g., "Order placed", "Payment completed").
    // 2. **Debugging**: Helps identify issues by recording order-related events.
    // 3. **Auditing**: Ensures traceability of actions taken on the order (who did what, when).
    // 4. **Customer Support**: Provides insights into the order's progress for customer service.
    // 5. **Reporting**: Analyzes the order process and identifies potential delays or issues.


