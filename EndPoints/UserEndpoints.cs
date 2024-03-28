using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAssignment3.Data;
using WebAssignment3.Models;

namespace WebAssignment3.EndPoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/User").WithTags(nameof(User));

            group.MapGet("/", async (MyDatabaseContext db, HttpContext context) =>
            {
                var users = await db.Users.ToListAsync();
                return new JsonResult(users);
            })
            .WithName("GetAllUsers")
            .WithOpenApi();

            group.MapGet("/{id}", async (int id, MyDatabaseContext db, HttpContext context) =>
            {
                var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id);
                if (user != null)
                {
                    return new JsonResult(user);
                }
                else
                {
                    return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("GetUserById")
            .WithOpenApi();

            group.MapPut("/{id}", async (int id, User user, MyDatabaseContext db, HttpContext context) =>
            {
                var existingUser = await db.Users.FindAsync(id);
                if (existingUser != null)
                {
                    existingUser.Email = user.Email;
                    existingUser.Password = user.Password;
                    existingUser.Username = user.Username;
                    existingUser.PurchaseHistory = user.PurchaseHistory;
                    existingUser.ShippingAddress = user.ShippingAddress;

                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "User updated successfully." });
                }
                else
                {
                    return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("UpdateUser")
            .WithOpenApi();

            group.MapPost("/", async (User user, MyDatabaseContext db, HttpContext context) =>
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                context.Response.StatusCode = StatusCodes.Status201Created;
                return new JsonResult(new { message = "User created successfully.", user });
            })
            .WithName("CreateUser")
            .WithOpenApi();

            group.MapDelete("/{id}", async (int id, MyDatabaseContext db, HttpContext context) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return new JsonResult(new { message = "User deleted successfully." });
                }
                else
                {
                    return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
                }
            })
            .WithName("DeleteUser")
            .WithOpenApi();
        }
    }
}
