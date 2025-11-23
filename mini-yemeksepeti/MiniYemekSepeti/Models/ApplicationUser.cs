using Microsoft.AspNetCore.Identity;

namespace MiniYemekSepeti.Models
{
    // The ApplicationUser class extends the IdentityUser class to customize user information.
    // This class is used for user management and authentication operations in ASP.NET Core Identity.
    public class ApplicationUser : IdentityUser
    {
        // Represents the user's first name. (Custom field)
        public string FirstName { get; set; } = "";

        // Represents the user's last name. (Custom field)
        public string LastName { get; set; } = "";

        // Represents the user's address. (Custom field)
        public string Address { get; set; } = "";

        // Represents the date and time when the user account was created. (Custom field)
        public DateTime CreatedAt { get; set; }
    }
}
