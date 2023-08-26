using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Football_Fun_Facts.Models;

namespace Football_Fun_Facts.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Football_Fun_Facts.Models.Facts>? Facts { get; set; }
    }
}