using System.ComponentModel.DataAnnotations;
namespace UrlShortener.Models;

public class Url 
{
    public string? Id { get; set; }  // No Required attribute

    [Required]
    [Url]
    public string? LongUrl { get; set; }  

    public string? ShortUrl { get; set; }  

    public DateTime GeneratedDate { get; set; }

    public string? SiteUserId {get; set;}
}
