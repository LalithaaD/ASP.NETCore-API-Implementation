using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAssignment3.Data;
using WebAssignment3.Models;

namespace WebAssignment3.EndPoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Product").WithTags(nameof(Product));

            group.MapGet("/", async (MyDatabaseContext db, HttpContext context) =>
            {
                var products = await db.Products.ToListAsync();
                return new JsonResult(products);
            })
            .WithName("GetAllProducts")
            .WithOpenApi();

            group.MapGet("/{id}", async (int id, MyDatabaseContext db, HttpContext context) =>
            {
                var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id);
                if (product != null)
                {
                    return new JsonResult(product);
                }
                else
                {
                    return new JsonResult(new { message = "Product not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("GetProductById")
            .WithOpenApi();

            group.MapPut("/{id}", async (int id, Product product, MyDatabaseContext db, HttpContext context) =>
            {
                var existingProduct = await db.Products.FindAsync(id);
                if (existingProduct != null)
                {
                    existingProduct.Description = product.Description;
                    existingProduct.Image = product.Image;
                    existingProduct.Pricing = product.Pricing;
                    existingProduct.ShippingCost = product.ShippingCost;

                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "Product updated successfully." });
                }
                else
                {
                    return new JsonResult(new { message = "Product not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("UpdateProduct")
            .WithOpenApi();

            group.MapPost("/", async (Product product, MyDatabaseContext db, HttpContext context) =>
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                context.Response.StatusCode = StatusCodes.Status201Created;
                return new JsonResult(new { message = "Product created successfully.", product });
            })
            .WithName("CreateProduct")
            .WithOpenApi();

            group.MapDelete("/{id}", async (int id, MyDatabaseContext db, HttpContext context) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "Product deleted successfully." });
                }
                else
                {
                    return new JsonResult(new { message = "Product not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("DeleteProduct")
            .WithOpenApi();
        }
    }
}
