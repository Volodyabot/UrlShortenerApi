using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<ShortUrlInfo> ShortUrlInfos { get; set; }
    }
}