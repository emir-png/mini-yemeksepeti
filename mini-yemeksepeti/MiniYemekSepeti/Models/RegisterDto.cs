using System.ComponentModel.DataAnnotations;

namespace MiniYemekSepeti.Models
{
    public class RegisterDto
    {
        // The first name of the user. Required field with a maximum length of 100 characters.
        [Required(ErrorMessage = "The First Name field is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";

        // The last name of the user. Required field with a maximum length of 100 characters.
        [Required(ErrorMessage = "The Last Name field is required"), MaxLength(100)]
        public string LastName { get; set; } = "";

        // The email address of the user. Required, must be a valid email format, and has a maximum length of 100 characters.
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = "";

        // The phone number of the user. Optional field with validation for phone number format and a maximum length of 20 characters.
        [Phone(ErrorMessage = "The format of the Phone Number is not valid"), MaxLength(20)]
        public string? PhoneNumber { get; set; }

        // The address of the user. Required field with a maximum length of 200 characters.
        [Required, MaxLength(200)]
        public string Address { get; set; } = "";

        // The password for the user account. Required field with a maximum length of 100 characters.
        [Required, MaxLength(100)]
        public string Password { get; set; } = "";

        // Confirm password to ensure the password entered matches. This field is required.
        [Required(ErrorMessage = "The Confirm Password field is required")]
        [Compare("Password", ErrorMessage = "Confirm Password and Password do not match")]
        public string ConfirmPassword { get; set; } = "";
    }
}

// Purpose of the RegisterDto class:
// 1. **User Registration**: Collects and validates necessary information to register a user (name, email, password, etc.).
// 2. **Password Validation**: Ensures that the user enters a matching password and confirmation password.
// 3. **Data Validation**: Checks that all required fields are provided, and that the entered data is in the correct format (e.g., valid email, valid phone number).
// 4. **User Onboarding**: Used to create new user accounts, ensuring that all required data is collected and validated before storing it in the database.
