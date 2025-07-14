using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

/* Since our RepositoryContext class is in a Repository project and not in the
main one, this class will help our application create a derived DbContext
instance during the design time which will help us with our migrations*/

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>().UseMySql(
            configuration.GetConnectionString("mysqlConnection"),
            new MySqlServerVersion(new Version(8, 0, 36)), // MySQL sürümünü belirt
            b => b.MigrationsAssembly("CompanyEmployees") // Migration'lar bu projede olacak
        );

        return new RepositoryContext(builder.Options);
    }
}
