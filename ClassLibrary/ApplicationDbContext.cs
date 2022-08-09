using ClassLibrary.Models.Database;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClassLibrary
{
    public class ApplicationDbContext : IdentityDbContext
    {
        
        public DbSet<Art> Arts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TextPage> TextPages { get; set; }
        public DbSet<Detail> Details { get; set; }

        private IConfiguration Configuration { get; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options) 
            => options.UseSqlite(
                $"Data Source={DatabaseName}", b => b.MigrationsAssembly("CollageArt"));
        

        private string DatabaseName => Configuration["Database"];
        

    }
}