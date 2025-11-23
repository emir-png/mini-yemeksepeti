using MiniYemekSepeti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MiniYemekSepeti.Services;

namespace MiniYemekSepeti.Controllers
{
    // This controller is only accessible by users with the "admin" role
    [Authorize(Roles = "admin")]
    [Route("/Admin/[controller]/{action=Index}/{id?}")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;

        // Constructor: Injecting UserManager, RoleManager, and DbContext into the controller
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._context = context;
        }

        // Displays a list of all users
        public IActionResult Index()
        {
            // Retrieve users ordered by creation date in descending order
            var users = userManager.Users.OrderByDescending(u => u.CreatedAt).ToList();
            return View(users);
        }

        // Displays the details of a specific user
        public async Task<IActionResult> Details(string? id)
        {
            // Redirect to Index if the user ID is not provided
            if (id == null)
            {
                return RedirectToAction("Index", "Users");
            }

            // Find the user by ID
            var appUser = await userManager.FindByIdAsync(id);

            // Redirect to Index if the user is not found
            if (appUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            // Fetch the user's roles and pass them to the view
            ViewBag.Roles = await userManager.GetRolesAsync(appUser);

            // Retrieve the list of available roles from the role manager
            var availableRoles = roleManager.Roles.ToList();
            var items = new List<SelectListItem>();

            // For each role, create a SelectListItem and set whether it's selected for the user
            foreach (var role in availableRoles)
            {
                items.Add(
                    new SelectListItem
                    {
                        Text = role.NormalizedName,
                        Value = role.Name,
                        Selected = await userManager.IsInRoleAsync(appUser, role.Name!),
                    });
            }

            // Pass the list of SelectListItems to the view
            ViewBag.SelectItems = items;

            return View(appUser);
        }

        // Edits the role of a user
        public async Task<IActionResult> EditRole(string? id, string? newRole)
        {
            // If the ID or role is not provided, redirect to the Index
            if (id == null || newRole == null)
            {
                return RedirectToAction("Index", "Users");
            }

            // Check if the role exists
            var roleExists = await roleManager.RoleExistsAsync(newRole);
            var appUser = await userManager.FindByIdAsync(id);

            // If user or role doesn't exist, redirect to Index
            if (appUser == null || !roleExists)
            {
                return RedirectToAction("Index", "Users");
            }

            // Ensure that admins can't modify their own roles
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser!.Id == appUser.Id)
            {
                TempData["ErrorMessage"] = "You cannot update your own role!";
                return RedirectToAction("Details", "Users", new { id });
            }

            // Remove current roles and add the new role for the user
            var userRoles = await userManager.GetRolesAsync(appUser);
            await userManager.RemoveFromRolesAsync(appUser, userRoles);
            await userManager.AddToRoleAsync(appUser, newRole);

            TempData["SuccessMessage"] = "User Role updated successfully";
            return RedirectToAction("Details", "Users", new { id });
        }

        // Deletes a user's account
        public async Task<IActionResult> DeleteAccount(string? id)
        {
            // Redirect to Index if the user ID is not provided
            if (id == null)
            {
                return RedirectToAction("Index", "Users");
            }

            // Find the user by ID
            var appUser = await userManager.FindByIdAsync(id);

            // Redirect to Index if the user is not found
            if (appUser == null)
            {
                return RedirectToAction("Index", "Users");
            }

            // Ensure that users can't delete their own account
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser!.Id == appUser.Id)
            {
                TempData["ErrorMessage"] = "You cannot delete your own account!";
                return RedirectToAction("Details", "Users", new { id });
            }

            // Call the stored procedure to delete the user and their associated data
            var userIdParam = new SqlParameter("@UserId", id);

            try
            {
                // Execute the stored procedure to delete the user
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteUserAccount @UserId", userIdParam);
                TempData["SuccessMessage"] = "User and associated data deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Unable to delete this account: {ex.Message}";
            }

            return RedirectToAction("Index", "Users");
        }

      
    }
}
