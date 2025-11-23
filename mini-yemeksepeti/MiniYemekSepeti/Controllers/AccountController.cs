using Microsoft.AspNetCore.Authorization; // For authorization operations
using Microsoft.AspNetCore.Identity; // For identity and user management
using Microsoft.AspNetCore.Mvc; // For MVC operations
using MiniYemekSepeti.Models;
using System.ComponentModel.DataAnnotations; // For model validation

namespace MiniYemekSepeti.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager; // For user management
        private readonly SignInManager<ApplicationUser> signInManager; // For sign-in/sign-out operations

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager; // Assigns the UserManager
            this.signInManager = signInManager; // Assigns the SignInManager
        }

        // Register page (GET)
        public IActionResult Register()
        {
            if (signInManager.IsSignedIn(User)) // If the user is already signed in, redirect to the home page
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // Registration process (POST)
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) // If model validation fails, reload the page
            {
                return View(registerDto);
            }

            var user = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email, // Username (set to email)
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                CreatedAt = DateTime.Now, // Creation date
            };

            var result = await userManager.CreateAsync(user, registerDto.Password); // Create and register the user

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "client"); // Assign "client" role to the user
                await signInManager.SignInAsync(user, false); // Sign in the user
                return RedirectToAction("Index", "Home"); // Redirect to the home page
            }

            foreach (var error in result.Errors) // If there is an error, add the error messages to the model state
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registerDto); // Reload the page
        }

        // Logout process
        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User)) // If the user is signed in, sign out
            {
                await signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Home"); // Redirect to the home page
        }

        // Login page (GET)
        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User)) // If the user is already signed in, redirect to the home page
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // Login process (POST)
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (signInManager.IsSignedIn(User)) // If the user is already signed in, redirect to the home page
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid) // If model validation fails, reload the page
            {
                return View(loginDto);
            }

            var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe, false); // Sign in the user

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home"); // Redirect to the home page
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login attempt."; // If login fails, display error message
            }

            return View(loginDto); // Reload the login page
        }

        // Profile page (GET)
        [Authorize] // Only signed-in users can access
        public async Task<IActionResult> ProfileAsync()
        {
            var appUser = await userManager.GetUserAsync(User); // Get the current user
            if (appUser == null)
            {
                return RedirectToAction("Index", "Home"); // If the user is not found, redirect to the home page
            }

            var profileDto = new ProfileDto()
            {
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email ?? "",
                PhoneNumber = appUser.PhoneNumber,
                Address = appUser.Address,
            };

            return View(profileDto); // Show the profile page
        }

        // Profile update process (POST)
        [HttpPost]
        [Authorize] // Only signed-in users can access
        public async Task<IActionResult> Profile(ProfileDto profileDto)
        {
            if (!ModelState.IsValid) // If model validation fails, display error message and reload the page
            {
                ViewBag.ErrorMessage = "Please fill all the required fields with valid values";
                return View(profileDto);
            }

            var appUser = await userManager.GetUserAsync(User); // Get the current user
            if (appUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            appUser.FirstName = profileDto.FirstName;
            appUser.LastName = profileDto.LastName;
            appUser.UserName = profileDto.Email;
            appUser.Email = profileDto.Email;
            appUser.PhoneNumber = profileDto.PhoneNumber;
            appUser.Address = profileDto.Address;

            var result = await userManager.UpdateAsync(appUser); // Save the update

            if (result.Succeeded)
            {
                ViewBag.SuccessMessage = "Profile updated successfully";
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to update the profile: " + result.Errors.First().Description; // If there is an error, display error message
            }

            return View(profileDto); // Reload the profile page
        }

        // Password change page (GET)
        [Authorize] // Only signed-in users can access
        public IActionResult Password()
        {
            return View(); // Show the password change page
        }
    }
}
