using AadhaarSplitterAPI.DbSets;
using Microsoft.EntityFrameworkCore;

namespace AadhaarSplitterAPI.Data
{
    /// <summary>
    /// Represents the application's database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the DbSet for Aadhar entities.
        /// </summary>
        public DbSet<Aadhar> Aadhar { get; set; }
    }
}
