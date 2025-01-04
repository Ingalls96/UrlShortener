using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class MvcUrlContext : DbContext
    {
        public MvcUrlContext (DbContextOptions<MvcUrlContext> options)
            : base(options)
        {
        }

        public DbSet<UrlShortener.Models.Url> Url {get; set;}
    }
}