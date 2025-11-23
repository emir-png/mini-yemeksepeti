using System.ComponentModel.DataAnnotations;

namespace MiniYemekSepeti.Models
{
    public class ProfileDto
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
    }
}

// Purpose of the ProfileDto class:
// 1. **User Profile Management**: Stores and validates user details such as name, email, phone number, and address.
// 2. **Data Validation**: Ensures that all necessary fields are filled in correctly (e.g., email format, phone number format).
// 3. **User Experience**: Provides a structured way to manage and update user profile information, with error messages if the fields are not correctly filled.
// 4. **Integration with User Data**: Often used when updating or retrieving user profile data in user management systems.
