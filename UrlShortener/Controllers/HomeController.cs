using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using System.Threading.Tasks;
using System.Text;
using UrlShortener.Data;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MvcUrlContext _context;

    public HomeController(ILogger<HomeController> logger, MvcUrlContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View(new Url());
    }

    //Post URL
    [HttpPost]
    public async Task<IActionResult> ShortenUrl(Url model)
    {
        // Explicitly clear out Id and ShortUrl before processing
        model.Id = null;
        model.ShortUrl = null;

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

            _logger.LogInformation($"Generated ShortUrl: {model.ShortUrl}");
            var newUrl = await _context.Url.FirstOrDefaultAsync();
            if (newUrl == null)
            {
                return NotFound();
            }
            return View("Index", newUrl);  // Return the model with the generated short URL
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
