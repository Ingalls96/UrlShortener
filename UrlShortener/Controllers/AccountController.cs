using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using UrlShortener.Data;
using UrlShortener.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace UrlShortener.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<SiteUser> _userManager;
    private readonly SignInManager<SiteUser> _signInManager;

    private MvcUrlContext _context;

    public AccountController(UserManager<SiteUser> userManager, SignInManager<SiteUser> signInManager, MvcUrlContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<IActionResult> UserList()
    {
        List<SiteUser> userList = await _context.SiteUser.ToListAsync<SiteUser>();
        return View(userList);
    }

    //GET Create page - Empty Site User
    public IActionResult Create()
    {
        return View(new SiteUser());
    }

    //POST Create Page - Create New User
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SiteUser newUser, string password)
    {
        if (ModelState.IsValid)
            {
                try
                {
                    var passwordValidationResult = ValidatePassword(password);
                    if (!passwordValidationResult.IsValid)
                    {
                        // Add password validation errors to ModelState
                        foreach (var error in passwordValidationResult.Errors)
                        {
                            ModelState.AddModelError("Password", error);
                        }
                        return View(newUser);
                    }

                    var result = await _userManager.CreateAsync(newUser, password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(UserList));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating user: " + ex.Message);
                }
            }
            return View(newUser);
    }

    // Password validation method
        private (bool IsValid, List<string> Errors) ValidatePassword(string password)
        {
            var errors = new List<string>();

            // Check password length
            if (password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters long.");
            }

            // Check for at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }

            // Check for at least one digit
            if (!password.Any(char.IsDigit))
            {
                errors.Add("Password must contain at least one number.");
            }

            // Check for at least one special character
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                errors.Add("Password must contain at least one special character.");
            }

            return (errors.Count == 0, errors);
        }


}