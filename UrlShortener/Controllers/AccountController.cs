using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using UrlShortener.Data;
using UrlShortener.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;


namespace UrlShortener.Controllers;
public class AccountController : Controller
{
    //UserManager and SignInManager member variables to access AccountController constructor
    private readonly UserManager<SiteUser> _userManager;
    private readonly SignInManager<SiteUser> _signInManager;

    //MvcUrlContext variable used for database connections in the controller
    private MvcUrlContext _context;

    public AccountController(UserManager<SiteUser> userManager, SignInManager<SiteUser> signInManager, MvcUrlContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    //UserList grabs list of users from database
    public async Task<IActionResult> UserList()
    {
        List<SiteUser> userList = await _context.Users.ToListAsync<SiteUser>();
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

                newUser.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
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

    //GET: Login
    public IActionResult Login()
    {
        return View(new Login());
    }

    //POST: Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Login model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
        }

        return View(model);
    }

    // GET: Logout
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    //GET UserDetails page
    [HttpGet]
    public async Task<IActionResult> UserDetails(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _context.Users
            .Include(u => u.Links)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    //DELETE USER FUNCTION - Get user info
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    //DELETE USER FUNCTION - Deletion of user

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return Redirect("/Account/UserList");
        }
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Redirect("/Account/UserList");
    }

    //DELETEURL - Grabs the Url requested by the user to delete
    public async Task<IActionResult> DeleteUrl(string urlId)
    {
        var url = await _context.Url.FindAsync(urlId);
        if(url == null)
        {
            return Redirect("/Account/UserList");
        }
        return View(url);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUrlConfirmed(string urlId)
    {
        if (string.IsNullOrEmpty(urlId))
        {
            return Redirect("/Account/UserList");
        }
        var url = await _context.Url.FindAsync(urlId);
        if (url == null)
        {
            return NotFound();
        }

        _context.Url.Remove(url);
        await _context.SaveChangesAsync();

        return RedirectToAction("UserDetails", "Account", new {id = url.SiteUserId});
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