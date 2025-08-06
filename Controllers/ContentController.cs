using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using NWebApp.Context;
using NWebApp.Models;

namespace NWebApp.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
[Route("api/[controller]")]
public class ContentController (DataContext context) : ControllerBase
{
    // [HttpGet("string")]
    // public string GetString() => "This is a string response";

    [HttpGet("object/{format?}")]
    [FormatFilter]
    [Produces("application/json", "application/xml")]
    public async Task<ProductBindingTarget?> GetProduct()
    {
        var product = await context.Products.FirstAsync();
        return new ProductBindingTarget()
        {
            CategoryId = product.CategoryId,
            Name = product.Name,
            Price = product.Price,
            SupplierId = product.SupplierId
        };
    }

    [HttpPost]
    [Consumes("application/json")]
    public string SaveJsonProduct(ProductBindingTarget product)
    {
        return $"Save in Json format {product.Name}";
    }

    [HttpGet("string")]
    [OutputCache(PolicyName = "30Secs")]
    [Produces("application/json")]
    public string GetDateString() => $"Current Time: {DateTime.Now.ToLongTimeString()}";

    // [HttpPost] 
    // [ Consumes("application/xml")]
    // public string SaveXmlProduct(ProductBindingTarget productItem)
    // {
    //     return $"Save in XML format {productItem.Name}";
    // }
}