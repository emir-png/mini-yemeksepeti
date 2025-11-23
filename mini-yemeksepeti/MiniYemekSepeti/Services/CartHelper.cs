using MiniYemekSepeti.Models;
using System.Text.Json;

namespace MiniYemekSepeti.Services
{
    public class CartHelper
    {
        // Retrieves the shopping cart as a dictionary from the cookies, where the key is the food ID and the value is the quantity.
        public static Dictionary<int, int> GetCartDictionary(HttpRequest request, HttpResponse response)
        {
            string cookieValue = request.Cookies["shopping_cart"] ?? "";  // Retrieve the cookie value for the shopping cart.

            try
            {
                // Decode the cart cookie (base64 encoded), then deserialize it into a dictionary of food ID and quantity.
                var cart = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cookieValue));
                Console.WriteLine("[CartHelper] cart=" + cookieValue + " -> " + cart);
                var dictionary = JsonSerializer.Deserialize<Dictionary<int, int>>(cart);
                if (dictionary != null)
                {
                    return dictionary;  // Return the deserialized dictionary.
                }
            }
            catch (Exception)
            {
                // In case of any error, we ignore it and proceed with returning an empty cart.
            }

            if (cookieValue.Length > 0)
            {
                // If the cookie value is invalid (failed to decode), delete the cookie.
                response.Cookies.Delete("shopping_cart");
            }

            return new Dictionary<int, int>();  // Return an empty dictionary if the cookie is invalid or not present.
        }

        // Returns the total size (number of items) in the shopping cart.
        public static int GetCartSize(HttpRequest request, HttpResponse response)
        {
            int cartSize = 0;

            var cartDictionary = GetCartDictionary(request, response);  // Get the current cart as a dictionary.
            foreach (var keyValuePair in cartDictionary)
            {
                cartSize += keyValuePair.Value;  // Sum the quantities of all items in the cart.
            }

            return cartSize;  // Return the total number of items in the cart.
        }

        // Retrieves the list of items in the cart from the dictionary and loads the corresponding food data.
        public static List<OrderItem> GetCartItems(HttpRequest request, HttpResponse response, ApplicationDbContext context)
        {
            var cartItems = new List<OrderItem>();  // List to store cart items.

            var cartDictionary = GetCartDictionary(request, response);  // Get the cart dictionary.

            foreach (var pair in cartDictionary)
            {
                int foodId = pair.Key;  // Food ID from the dictionary.
                int quantity = pair.Value;  // Quantity of the food item in the cart.

                var food = context.Food.Find(foodId);  // Find the food item in the database by ID.
                if (food == null) continue;  // If the food item doesn't exist, skip it.

                // Create a new OrderItem object to represent this food item in the cart.
                var item = new OrderItem
                {
                    Quantity = quantity,
                    UnitPrice = food.Price,
                    Food = food,
                };

                cartItems.Add(item);  // Add the item to the cart items list.
            }

            return cartItems;  // Return the list of cart items.
        }

        // Calculates the subtotal (total price) of all items in the cart.
        public static decimal GetSubtotal(List<OrderItem> cartItems)
        {
            decimal subtotal = 0;

            foreach (var item in cartItems)
            {
                subtotal += item.Quantity * item.UnitPrice;  // Multiply quantity by unit price for each item and sum them up.
            }

            return subtotal;  // Return the calculated subtotal.
        }
    }
}
