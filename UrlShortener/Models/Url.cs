using System.ComponentModel.DataAnnotations;
using UrlShortener.Models.Identity;
namespace UrlShortener.Models;


//Url is the object used to create the URLS as well as the shortened version of the URLs on the web app
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

    public string? UrlName {get; set;}
}
