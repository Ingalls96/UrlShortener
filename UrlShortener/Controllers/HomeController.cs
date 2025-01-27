using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using System.Threading.Tasks;
using System.Text;
using UrlShortener.Data;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace UrlShortener.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MvcUrlContext _context;

    private readonly UserManager<SiteUser> _userManager;

    public HomeController(ILogger<HomeController> logger, MvcUrlContext context, UserManager<SiteUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View(new Url());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //Post URL
    [HttpPost]
    public async Task<IActionResult> ShortenUrl(Url model)
    {

        var user = await _userManager.GetUserAsync(User);

        // Log the ModelState validity
        _logger.LogInformation($"ModelState is valid: {ModelState.IsValid}");

        // If the ModelState is invalid, log each validation error
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogInformation($"ModelState Error: {error.ErrorMessage}");
            }
        }

        // Check if the LongUrl is provided and is valid
        if (!string.IsNullOrEmpty(model.LongUrl))
        {
            // Generate the Id and ShortUrl if the LongUrl is valid
            Guid guid = Guid.NewGuid();
            string shortId =  ToBase62(guid);
            model.Id = shortId;
            model.ShortUrl = $"https://short.ly/{model.Id}";
            model.GeneratedDate = DateTime.Now;
            model.UrlName = model.UrlName;
            if (User.Identity.IsAuthenticated)
            {
                if(user != null)
                {
                    model.SiteUserId = user.Id;
                    user.Links.Add(model);
                }
            }

            //Save URL to database
            _context.Add(model);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Generated ShortUrl: {model.ShortUrl}");

            return View("Index", model);  // Return the model with the generated short URL
        }

        // Log a warning and return the model if there is an issue
        _logger.LogWarning("Model is invalid or LongUrl is empty.");
        return View("Index", model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public static string ToBase62(Guid guid)
    {
        const string base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        byte[] guidBytes = guid.ToByteArray();
        var builder = new StringBuilder();

        // Convert the bytes into a Base62 string
        ulong value = BitConverter.ToUInt64(guidBytes, 0);  // Using the first 8 bytes of the GUID
        while (value > 0)
        {
            builder.Insert(0, base62Chars[(int)(value % 62)]);
            value /= 62;
        }

        return builder.ToString();
    }
}
