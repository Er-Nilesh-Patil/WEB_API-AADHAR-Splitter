using AadhaarSplitterAPI.DbSets;
using Microsoft.EntityFrameworkCore;

namespace AadhaarSplitterAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

         public DbSet<Aadhar> Aadhaars { get; set; }
    }
}
