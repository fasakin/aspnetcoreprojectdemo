using Microsoft.AspNetCore.Mvc;
using NWebApp.Models;

namespace NWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Product> GetProducts()
    {
        return
        [
            new Product() { Name = "Product #1" },
            new Product() { Name = "Product #" }
        ];
    }

    [HttpGet("{id}")]
    public Product GetProduct()
    {
        return new Product() { ProductId = 1, Name = "Test Product" };
    }
}