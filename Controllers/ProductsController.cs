using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NWebApp.Context;
using NWebApp.Models;

namespace NWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("FixedWindows")]
public class ProductsController : ControllerBase
{
    private DataContext _dataContext;

    public ProductsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public IAsyncEnumerable<Product> GetProducts()
    {
        return _dataContext.Products.AsAsyncEnumerable();
    }

    [HttpGet("{id}")]
    [DisableRateLimiting]
    public async Task<IActionResult> GetProduct(long id, [FromServices] ILogger<ProductsController> logger)
    {
        logger.LogInformation("GetProduct Action Invoked");
       var product = await _dataContext.Products.FindAsync(id);
       if (product == null)
           return NotFound();
       return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> SaveProduct( ProductBindingTarget product)
    {
      
        var prod = product.ToProduct();
        await _dataContext.Products.AddAsync(prod);
        await _dataContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new {id = prod.ProductId}, prod);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(long id,  ProductBindingTarget product)
    {
        var prod = await  _dataContext.Products.FindAsync(id);

        _dataContext.Products.Update(product.ToProduct());
        await _dataContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task DeleteProduct(long id)
    {
        var product = await _dataContext.Products.FindAsync(id);
        if (product == null)
        {
            return;
        }

        _dataContext.Products.Remove(product);
        await _dataContext.SaveChangesAsync();
    }

    [HttpGet("redirect")]
    public IActionResult GetRedirection()
    {
        return RedirectToAction(nameof(GetProduct), new { id = 2 });
    }
}