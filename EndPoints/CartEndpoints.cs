using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using WebAssignment3.Data;
using WebAssignment3.Models;
namespace WebAssignment3.EndPoints;

public static class CartEndpoints
{
    public static void MapCartEndpoints (this IEndpointRouteBuilder routes)
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

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Cart cart, MyDatabaseContext db) =>
        {
            var affected = await db.Carts
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, cart.Id)
                    .SetProperty(m => m.UserId, cart.UserId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCart")
        .WithOpenApi();

        group.MapPost("/", async (Cart cart, MyDatabaseContext db) =>
        {
            db.Carts.Add(cart);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Cart/{cart.Id}",cart);
        })
        .WithName("CreateCart")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, MyDatabaseContext db) =>
        {
            var affected = await db.Carts
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCart")
        .WithOpenApi();
    }
}
