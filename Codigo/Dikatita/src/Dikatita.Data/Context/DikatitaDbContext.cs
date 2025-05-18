using Dikatita.Business.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dikatita.Data.Context;

public class DikatitaDbContext : IdentityDbContext
{

    public DikatitaDbContext(DbContextOptions options) : base (options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<MovEstoque> MovEstoque { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DikatitaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties()
                         .Where(p => p.ClrType == typeof(string))))
        {
            property.SetColumnType("varchar(100)");
        }
    }

}