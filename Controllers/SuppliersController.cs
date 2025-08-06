using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NWebApp.Context;
using NWebApp.Models;

namespace NWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController(DataContext dataContext) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSupplier(long id)
    {
       var supplier = await dataContext.Suppliers.Include(x => x.Products)
            .FirstOrDefaultAsync(s => s.SupplierId == id);

       if (supplier == null)
       {
           return NotFound();
       }

       if (supplier.Products != null)
       {
           foreach (var product in supplier.Products)
           {
               product.Supplier = null;
           }
       }

       return Ok(supplier);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PatchSupplier(long id, JsonPatchDocument<Supplier> doc)
    {
        var supplier = await dataContext.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }
        doc.ApplyTo(supplier);
        await dataContext.SaveChangesAsync();

        return Ok(supplier);
    }
}