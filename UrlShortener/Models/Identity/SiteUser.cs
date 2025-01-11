using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Models;

namespace UrlShortener.Models.Identity
{
    public class SiteUser : IdentityUser
    {
        [Key]
        public override string Id {get; set;}

        [Required]
        [EmailAddress]
        public override string? Email {get; set;}

        public override string? UserName {get; set;}

        public string? FirstName {get; set;}

        public string? LastName {get; set;}

        public ICollection<Url>? Links {get; set;} = new List<Url>();
 
    }
}