namespace MiniYemekSepeti.Models
{
    // The Notification class represents a notification sent to a user.
    // It contains properties such as the message, user ID, timestamp, and read status.
    public class Notification
    {
        // The unique identifier for the notification. Typically used as the primary key.
        public int Id { get; set; }

        // The ID of the user who is receiving the notification.
        // This would typically be linked to a User entity in the system.
        public string UserId { get; set; }  // Kullanıcı ID

        // The message of the notification. This could be a text message providing important information.
        public string NotificationMessage { get; set; }  // Bildirim mesajı

        // The date and time when the notification was created.
        // This is useful for sorting and showing notifications in chronological order.
        public DateTime CreatedAt { get; set; }  // Bildirimin oluşturulma tarihi

        // Indicates whether the notification has been read by the user.
        // If true, the user has already viewed the notification; if false, the notification is unread.
        public bool IsRead { get; set; }  // Okundu/Okunmadı durumu

        // The name of the customer associated with the notification (optional).
        // This might be relevant if the notification pertains to a customer-related action.
        public string? CustomerName { get; set; }  // Müşteri Adı

        // The ID of the order related to this notification (optional).
        // This could be used if the notification is tied to a specific order.
        public int? OrderId { get; set; }  // Sipariş ID

        // The status of the order associated with this notification (optional).
        // This could represent the current status of the order (e.g., "Pending", "Delivered").
        public string? OrderStatus { get; set; }  // Sipariş Durumu

        // The date and time when the order associated with the notification was placed (optional).
        // This helps give context to the notification if it is related to a specific order.
        public DateTime? OrderDate { get; set; }  // Sipariş Tarihi
    }
}
