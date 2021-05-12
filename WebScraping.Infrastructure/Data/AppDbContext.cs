using Microsoft.EntityFrameworkCore;
using WebScraping.Core.Entities;

namespace WebScraping.Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<FileInformation> FileInformation { get; set; }
    }
}