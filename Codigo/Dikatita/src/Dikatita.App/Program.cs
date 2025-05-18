using Dikatita.Business.Interfaces;
using Dikatita.Data.Context;
using Dikatita.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dikatita.App;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Guardando a connection string do arquivo appSettings.json
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<DikatitaDbContext>(options =>
        options.UseSqlite(connectionString, b => b.MigrationsAssembly("Dikatita.Data")));

        // Adicionando o Identity
        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DikatitaDbContext>();

        builder.Services.AddDbContext<DikatitaDbContext>();

        //Protegendo as Actions com ValidateAntiforgeryToken
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });

        builder.Services.AddControllersWithViews();

        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddScoped<DikatitaDbContext>();
        builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
        builder.Services.AddScoped<IMovEstoqueRepository, MovEstoqueRepository>();


        // Gerando a APP
        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.MapRazorPages();

        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}