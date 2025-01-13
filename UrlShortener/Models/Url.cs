using System.ComponentModel.DataAnnotations;
using UrlShortener.Models.Identity;
namespace UrlShortener.Models;

public class Url 
{
    public string? Id { get; set; }

    [Required]
    [Url]
    public string? LongUrl { get; set; }  

    public string? ShortUrl { get; set; }  

    public DateTime GeneratedDate { get; set; }

    public string? SiteUserId {get; set;}

    public SiteUser SiteUser {get; set;}
}
