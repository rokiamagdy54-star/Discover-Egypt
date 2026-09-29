using Tourism.Core.Entities;
using Tourism.Repository.Data.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Tourism.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedRolesAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Tourist", "Guide", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@discoveregypt.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    NationalityId = 1
                };

                await userManager.CreateAsync(user, "Admin@123456");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                new Category
                {
                    Name = "Historical",
                    Title = "Historical Places",
                    Description = "Historical destinations in Egypt"
                },
                new Category
                {
                    Name = "Museum",
                    Title = "Museums",
                    Description = "Museums and exhibitions"
                },
                new Category
                {
                    Name = "Religious",
                    Title = "Religious Places",
                    Description = "Mosques, churches and religious sites"
                },
                new Category
                {
                    Name = "Beach",
                    Title = "Beaches",
                    Description = "Beach destinations"
                },
                new Category
                {
                    Name = "Nature",
                    Title = "Nature",
                    Description = "Natural attractions"
                },
                new Category
                {
                    Name = "Desert",
                    Title = "Desert",
                    Description = "Desert adventures"
                },
                new Category
                {
                    Name = "Shopping",
                    Title = "Shopping",
                    Description = "Shopping destinations"
                },
                new Category
                {
                    Name = "Other",
                    Title = "Other",
                    Description = "Other categories"
                }
);
            }

            await context.SaveChangesAsync();
        }
    }
}