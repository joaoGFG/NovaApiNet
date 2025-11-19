using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NovaData
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Connection string para design-time (migrations)
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=NovaCareerDB;Trusted_Connection=True;MultipleActiveResultSets=true"
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
