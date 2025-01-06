using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using UrlShortener.Data;
using UrlShortener.Models.Identity;

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


}