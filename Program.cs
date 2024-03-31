using Medical_clinic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Medical_clinic.Models;
using Medical_clinic.Observer;

namespace Medical_clinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<MedicalFacility>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            /*
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "measure-sequential-time",
                    pattern: "measure-sequential-time",
                    defaults: new { controller = "Appointments", action = "MeasureSequentialTime" });

                endpoints.MapControllerRoute(
                    name: "measure-parallel-time",
                    pattern: "measure-parallel-time",
                    defaults: new { controller = "Appointments", action = "MeasureParallelTime" });

            });
            */

            app.Run();
        }
    }
}