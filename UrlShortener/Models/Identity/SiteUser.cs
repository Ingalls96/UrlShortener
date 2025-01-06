using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Models;

namespace UrlShortener.Models.Identity
{
    public class SiteUser : IdentityUser
    {
        [Key]
        public override required string Id {get; set;}

        [Required]
        [EmailAddress]
        public override required string Email {get; set;}

        public override required string UserName {get; set;}

        public required string FirstName {get; set;}

        public required string LastName {get; set;}

        public ICollection<Url>? Links {get; set;}
 
    }
}