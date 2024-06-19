using Api_Fiorella_HomeTask.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api_Fiorella_HomeTask.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Slider> Sliders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new CityConfigurations()); eger tek tek configuration etmek isteyirkse bunu yaziriq
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
         
      



        }
    }
}
