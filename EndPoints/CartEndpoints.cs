using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using WebAssignment3.Data;
using WebAssignment3.Models;

namespace WebAssignment3.EndPoints
{
    public static class CartEndpoints
    {
        public static void MapCartEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Cart").WithTags(nameof(Cart));

            group.MapGet("/", async (MyDatabaseContext db) =>
            {
                return await db.Carts.ToListAsync();
            })
            .WithName("GetAllCarts")
            .WithOpenApi();

            group.MapGet("/{id}", async Task<Results<Ok<Cart>, NotFound>> (int id, MyDatabaseContext db) =>
            {
                return await db.Carts.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Id == id)
                    is Cart model
                        ? TypedResults.Ok(model)
                        : TypedResults.NotFound();
            })
            .WithName("GetCartById")
            .WithOpenApi();

            // Update the PUT endpoint to handle the update operation
            group.MapPut("/{id}", async (int id, Cart updatedCart, MyDatabaseContext db) =>
            {
                var existingCart = await db.Carts.FindAsync(id);
                if (existingCart == null)
                {
                    return Results.NotFound();
                }

                // Update cart properties with the provided data
                existingCart.UserId = updatedCart.UserId;
                // Update other properties as needed

                db.Carts.Update(existingCart);
                await db.SaveChangesAsync();

                return Results.Ok(); // Return 200 OK if the cart is updated successfully
            })
            .WithName("UpdateCart")
            .WithOpenApi();

            group.MapPost("/", async (Cart cart, MyDatabaseContext db) =>
            {
                db.Carts.Add(cart);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/Cart/{cart.Id}", cart);
            })
            .WithName("CreateCart")
            .WithOpenApi();

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, MyDatabaseContext db) =>
            {
                var existingCart = await db.Carts.FindAsync(id);
                if (existingCart == null)
                {
                    return TypedResults.NotFound();
                }

                db.Carts.Remove(existingCart);
                await db.SaveChangesAsync();

                return TypedResults.Ok();
            })
            .WithName("DeleteCart")
            .WithOpenApi();
        }
    }
}
