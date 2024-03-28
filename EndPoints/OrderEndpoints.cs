using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAssignment3.Data;
using WebAssignment3.Models;

namespace WebAssignment3.EndPoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Order").WithTags(nameof(Order));

            group.MapGet("/", async (MyDatabaseContext db, HttpContext context) =>
            {
                var orders = await db.Orders.ToListAsync();
                return new JsonResult(orders);
            })
            .WithName("GetAllOrders")
            .WithOpenApi();

            group.MapGet("/{id}", async (int id, MyDatabaseContext db, HttpContext context) =>
            {
                var order = await db.Orders.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id);
                if (order != null)
                {
                    return new JsonResult(order);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return new JsonResult(new { message = "Order not found" });
                }
            })
            .WithName("GetOrderById")
            .WithOpenApi();

            group.MapPut("/{id}", async (int id, Order order, MyDatabaseContext db, HttpContext context) =>
            {
                var existingOrder = await db.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
                if (existingOrder != null)
                {
                    existingOrder.UserId = order.UserId;
                    existingOrder.OrderDate = order.OrderDate;
                    existingOrder.TotalAmount = order.TotalAmount;

                    // Update order items
                    foreach (var newItem in order.OrderItems)
                    {
                        var existingItem = existingOrder.OrderItems.FirstOrDefault(oi => oi.Id == newItem.Id);
                        if (existingItem != null)
                        {
                            // Update existing item
                            existingItem.ProductId = newItem.ProductId;
                            existingItem.Quantity = newItem.Quantity;
                            existingItem.Price = newItem.Price;
                        }
                        else
                        {
                            // Add new item
                            existingOrder.OrderItems.Add(newItem);
                        }
                    }

                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "Order updated successfully." });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return new JsonResult(new { message = "Order not found" });
                }
            })
             .WithName("UpdateOrder")
             .WithOpenApi();


            group.MapPost("/", async (Order order, MyDatabaseContext db, HttpContext context) =>
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                context.Response.StatusCode = StatusCodes.Status201Created;
                return new JsonResult(new { message = "Order created successfully." });
            })
            .WithName("CreateOrder")
            .WithOpenApi();

            group.MapDelete("/{id}", async (int id, MyDatabaseContext db, HttpContext context) =>
            {
                var order = await db.Orders.FindAsync(id);
                if (order != null)
                {
                    db.Orders.Remove(order);
                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "Order deleted successfully." });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return new JsonResult(new { message = "Order not found" });
                }
            })
            .WithName("DeleteOrder")
            .WithOpenApi();
        }
    }
}
