using Dikatita.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

//public class DikatitaDbContextFactory : IDesignTimeDbContextFactory<DikatitaDbContext>
//{
//    public DikatitaDbContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<DikatitaDbContext>();
//        var connectionString = "Data Source=dikatita.db";
//        optionsBuilder.UseSqlite(connectionString, b => b.MigrationsAssembly("Dikatita.Data"));
//        return new DikatitaDbContext(optionsBuilder.Options);
//    }
//}