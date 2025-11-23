using System.ComponentModel.DataAnnotations;

namespace MiniYemekSepeti.Models
{
    // The LoginDto class is used to transfer data related to a user login request.
    // It contains the email, password, and an optional RememberMe property.
    public class LoginDto
    {
        // The email address associated with the user's account. This field is required for login.
        [Required]
        public string Email { get; set; } = "";

        // The password for the user's account. This field is required for login.
        [Required]
        public string Password { get; set; } = "";

        // A flag indicating whether the user wants to be remembered. If true, the login session is kept persistent (e.g., through a "Remember Me" feature).
        // This field is optional and defaults to false if not specified.
        public bool RememberMe { get; set; }
    }
}
