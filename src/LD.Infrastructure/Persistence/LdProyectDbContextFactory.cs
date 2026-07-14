using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LD.Infrastructure.Persistence;

public class LdProyectDbContextFactory : IDesignTimeDbContextFactory<LdProyectDbContext>
{
    public LdProyectDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LdProyectDbContext>();

        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Server=localhost\\SQLEXPRESS;Database=Warehouse_System;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;";

        optionsBuilder.UseSqlServer(connectionString, b =>
            b.MigrationsAssembly(typeof(LdProyectDbContext).Assembly.GetName().Name));

        return new LdProyectDbContext(optionsBuilder.Options);
    }
}
