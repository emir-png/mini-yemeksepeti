using System.ComponentModel.DataAnnotations;

namespace MiniYemekSepeti.Models
{
    // The CheckoutDto class is used to transfer data related to the checkout process.
    // It includes delivery address, building number, and payment method information.
    public class CheckoutDto
    {
        // Specifies that the Delivery Address is a required field, with a custom error message.
        // The maximum length allowed for the address is 200 characters.
        [Required(ErrorMessage = "The Delivery Address is required.")]
        [MaxLength(200)]
        public string DeliveryAddress { get; set; } = "";

        // Specifies that the Building Number is required and must be between 1 and 30.
        // If the input is outside this range, a custom error message will be shown.
        [Required(ErrorMessage = "The Building Number is required.")]
        [Range(1, 30, ErrorMessage = "Please select a valid building number between 1 and 30.")]
        public int BuildingNumber { get; set; }

        // Represents the payment method chosen by the user for the checkout process.
        // This field is not required (no validation attribute is applied).
        public string PaymentMethod { get; set; } = "";
    }
}
