namespace MiniYemekSepeti.Models
{
    public class ShoppingSearchModel
    {
        // The search keyword that the user enters to search for specific foods or items.
        public string? Search { get; set; }

        // The name of the restaurant to filter search results by a specific restaurant.
        public string? Restaurant { get; set; }

        // The category to filter search results by a specific type of food or item.
        public string? Category { get; set; }

        // The sorting option selected by the user to determine how search results are ordered.
        public string? Sort { get; set; }
    }
}
