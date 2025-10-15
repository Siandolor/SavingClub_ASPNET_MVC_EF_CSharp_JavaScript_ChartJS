// =============================================================================
// FILE: Program.cs
// PROJECT: SavingsClub
// =============================================================================
// Entry point of the SavingsClub ASP.NET Core MVC application.
// Configures essential services, database context, middleware pipeline,
// and default routing for all controllers and views.
// =============================================================================

using Microsoft.EntityFrameworkCore;
using SavingsClub.Data;

namespace SavingsClub
{
    public class Program
    {
        // ---------------------------------------------------------------------
        // METHOD: Main
        // ---------------------------------------------------------------------
        // Application entry point. Builds the web application, configures the
        // service container, sets up middleware, and starts the runtime host.
        // ---------------------------------------------------------------------
        public static void Main(string[] args)
        {
            // ================================================================
            // APP INITIALIZATION
            // ================================================================
            var builder = WebApplication.CreateBuilder(args);

            // ================================================================
            // SERVICE CONFIGURATION
            // ================================================================
            // Adds MVC controller and view support.
            builder.Services.AddControllersWithViews();

            // Registers the EF Core database context with SQLite as provider.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ================================================================
            // BUILD APPLICATION
            // ================================================================
            var app = builder.Build();

            // ================================================================
            // DATABASE INITIALIZATION
            // ================================================================
            // Ensures the database is created on first run. 
            // (No seeding logic is included here.)
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            // ================================================================
            // ERROR HANDLING & SECURITY
            // ================================================================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // ================================================================
            // MIDDLEWARE PIPELINE
            // ================================================================
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            // ================================================================
            // ROUTING CONFIGURATION
            // ================================================================
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // ================================================================
            // START APPLICATION
            // ================================================================
            app.Run();
        }
    }
}
