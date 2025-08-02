using Microsoft.EntityFrameworkCore;
using NWebApp.Models;

namespace NWebApp.Context;

public class SeedData
{
    public static async Task SeedDatabase(DataContext context)
    {
        if(context.Database.GetPendingMigrations().Any())
            await context.Database.MigrateAsync();
       
        
        if (!context.Products.Any() &&
            !context.Categories.Any() &&
            !context.Suppliers.Any())
        {
            var s1 = new Supplier() { Name = "Splash Dudes", City = "San Jose" };
            var s2 = new Supplier() { Name = "Soccer Town", City = "Chicago" };
            var s3 = new Supplier() { Name = "Chess Co", City = "New York" };

            var c1 = new Category() { Name = "Watersports" };
            var c2 = new Category() { Name = "Soccer" };
            var c3 = new Category() { Name = "Chess" };

            await context.Products.AddRangeAsync(
                new Product(){Name = "Kayak", Price = 275M, Category = c1, Supplier = s1},
                new Product(){Name = "Lifejacket", Price = 48.95M, Category = c1, Supplier = s1},
                new Product(){Name = "Soccer Ball", Price = 19.50M, Category = c2, Supplier = s2},
                new Product(){Name = "Corner Flag", Price = 34.95M, Category = c2, Supplier = s2},
                new Product(){Name = "Stadium", Price = 79500, Category = c2, Supplier = s2},
                new Product(){Name = "Thinking Cap", Price = 16, Category = c3, Supplier = s3},
                new Product(){Name = "Unsteady Chair", Price = 29.95M, Category = c3, Supplier = s3},
                new Product(){Name = "Human Chess Board", Price = 75, Category = c3, Supplier = s3},
                new Product(){Name = "Bling-Bling King", Price = 1200, Category = c3, Supplier = s3}
            );

            await context.SaveChangesAsync();
        }
    }
} 