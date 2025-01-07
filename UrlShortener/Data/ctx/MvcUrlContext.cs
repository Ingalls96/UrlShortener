using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.WebEncoders.Testing;
using UrlShortener.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UrlShortener.Models.Identity;

namespace UrlShortener.Data
{
    public class MvcUrlContext : IdentityDbContext<SiteUser>
    {
        public MvcUrlContext (DbContextOptions<MvcUrlContext> options)
            : base(options)
        {
        }

        public DbSet<UrlShortener.Models.Url> Url {get; set;}

        public DbSet<UrlShortener.Models.Identity.SiteUser> SiteUser {get; set;}

        // protected override void OnModelCreating(ModelBuilder model)
        // {
        //     base.OnModelCreating(model);

        //     model.Entity<IdentityUserRole<string>>()
        //         .HasIndex(r => r.UserId)
        //         .IsUnique(false);
            
        //     model.Entity<SiteUser>().HasData(
        //         new SiteUser{Id = Guid.NewGuid().ToString(), Email = "Test@Test.com", FirstName = "Dummy", LastName = "DumDum", UserName = "testAccount"}
        //     );
                
        // }
    }
}