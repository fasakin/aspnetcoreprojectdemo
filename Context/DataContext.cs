using Microsoft.EntityFrameworkCore;
using NWebApp.Models;

namespace NWebApp.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options):
        base(options) {}

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Category> Categories => Set<Category>();
}