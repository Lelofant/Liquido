using Liquido.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Liquido.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {

            await SeedRolesAsync(roleManager);
            await SeedAdminUserAsync(userManager);
            await SeedCategoryAsync(context);
            await SeedProductsAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));

                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@liquido.bg";
            string adminPassword = "admin321";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Liquido",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        private static async Task SeedCategoryAsync(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync())
                return;

            var categories = new List<Category>
                {
                    new() { Name = "Natural Water",   Description = "Clean, fresh water straight from natural springs you can trust.", IsActive = true },
                    new() { Name = "Sparkling Water", Description = "Crisp and bubbly water for when you want something a bit more refreshing.", IsActive = true },
                    new() { Name = "Flavored Water",  Description = "Lightly flavored water with no sugar, just a hint of taste.", IsActive = true },
                    new() { Name = "Dispensers",      Description = "Everything you need to store and pour your water at home or work.", IsActive = true },
                    new() { Name = "Juices",          Description = "Tasty, freshly made juices packed with natural flavor.", IsActive = true },
                };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

        }

        private static async Task SeedProductsAsync(ApplicationDbContext context)
        {
            if (await context.Products.AnyAsync())
                return;

            var naturalId = (await context.Categories.FirstAsync(c => c.Name == "Natural Water")).Id;
            var sparklingId = (await context.Categories.FirstAsync(c => c.Name == "Sparkling Water")).Id;
            var flavoredId = (await context.Categories.FirstAsync(c => c.Name == "Flavored Water")).Id;
            var dispenserId = (await context.Categories.FirstAsync(c => c.Name == "Dispensers")).Id;
            var juiceId = (await context.Categories.FirstAsync(c => c.Name == "Juices")).Id;
            var products = new List<Product>
            {

                new()
                {
                    Name          = "Изворна вода Рила 0.5L",
                    Description   = "Чиста вода от Рила – лека, свежа и идеална за всеки ден.",
                    Price         = 0.89m,
                    StockQuantity = 500,
                    Volume        = "500ml",
                    CategoryId    = naturalId,
                    IsActive      = true,
                    IsFeatured    = true
                },

                new()
                {
                    Name          = "Изворна вода Рила 1.5L",
                    Description   = "Удобен семеен размер – перфектен за вкъщи или в офиса.",
                    Price         = 1.29m,
                    StockQuantity = 400,
                    Volume        = "1.5L",
                    CategoryId    = naturalId,
                    IsActive      = true,
                    IsFeatured    = true
                },

                new()
                {
                    Name          = "Минерална вода Балкан 5L",
                    Description   = "Натурална минерална вода с балансиран вкус – подходяща за ежедневна употреба.",
                    Price         = 3.49m,
                    StockQuantity = 150,
                    Volume        = "5L",
                    CategoryId    = naturalId,
                    IsActive      = true,
                    IsFeatured    = false
                },

                new()
                {
                    Name          = "Вода за диспенсър 19L",
                    Description   = "Голям галон за диспенсър – практично решение за дома и офиса.",
                    Price         = 8.99m,
                    StockQuantity = 80,
                    Volume        = "19L",
                    CategoryId    = naturalId,
                    IsActive      = true,
                    IsFeatured    = true
                },
                
                new()
                {
                    Name          = "Газирана вода Искра 0.5L",
                    Description   = "Леко газирана вода с приятен и освежаващ вкус.",
                    Price         = 1.19m,
                    StockQuantity = 300,
                    Volume        = "500ml",
                    CategoryId    = sparklingId,
                    IsActive      = true,
                    IsFeatured    = false
                },

                new()
                {
                    Name          = "Газирана вода Искра 1.5L",
                    Description   = "Подходяща за хранене или събирания – свежест във всяка глътка.",
                    Price         = 1.79m,
                    StockQuantity = 250,
                    Volume        = "1.5L",
                    CategoryId    = sparklingId,
                    IsActive      = true,
                    IsFeatured    = true
                },
                
                new()
                {
                    Name          = "Вода с вкус на цитрус 0.5L",
                    Description   = "Лека вода с освежаващ цитрусов вкус – без добавена захар.",
                    Price         = 1.39m,
                    StockQuantity = 200,
                    Volume        = "500ml",
                    CategoryId    = flavoredId,
                    IsActive      = true,
                    IsFeatured    = false
                },

                new()
                {
                    Name          = "Вода с вкус на горски плодове 0.5L",
                    Description   = "Освежаваща вода с лек вкус на горски плодове – идеална за разнообразие.",
                    Price         = 1.39m,
                    StockQuantity = 180,
                    Volume        = "500ml",
                    CategoryId    = flavoredId,
                    IsActive      = true,
                    IsFeatured    = true
                },
                
                new()
                {
                    Name          = "Диспенсър за вода АкваДом",
                    Description   = "Компактен диспенсър с топла и студена вода – удобен за всеки дом или офис.",
                    Price         = 89.99m,
                    StockQuantity = 25,
                    Volume        = null,
                    CategoryId    = dispenserId,
                    IsActive      = true,
                    IsFeatured    = true
                },
                
                new()
                {
                    Name          = "Портокалов сок прясно изцеден 330ml",
                    Description   = "100% портокалов сок без добавки – свеж и натурален вкус.",
                    Price         = 2.99m,
                    StockQuantity = 100,
                    Volume        = "330ml",
                    CategoryId    = juiceId,
                    IsActive      = true,
                    IsFeatured    = true
                },
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

        }
    }
}
