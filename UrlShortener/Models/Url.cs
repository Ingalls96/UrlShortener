using System.ComponentModel.DataAnnotations;
namespace UrlShortener.Models;

public class Url 
{
    public string Id { get; set; }  // No Required attribute

    [Required]
    [Url]
    public string LongUrl { get; set; }  // Required for user input

    public string ShortUrl { get; set; }  // No Required attribute

    public DateTime GeneratedDate { get; set; }
}
