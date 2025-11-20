using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NovaData
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Connection string Oracle para design-time (migrations)
            optionsBuilder.UseOracle(
                "User Id=SEU_ID;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL;"
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
