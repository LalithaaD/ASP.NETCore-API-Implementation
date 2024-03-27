using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using WebAssignment3.Data;
using WebAssignment3.Models;
using System.Threading.Tasks;

namespace WebAssignment3.EndPoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/User").WithTags(nameof(User));

            group.MapGet("/", async context =>
            {
                using var db = context.RequestServices.GetRequiredService<MyDatabaseContext>();
                var users = await db.Users.ToListAsync();
                await context.Response.WriteAsJsonAsync(users); // Write the response directly to the client
            })
            .WithName("GetAllUsers")
            .WithOpenApi();

            group.MapGet("/{id}", async context =>
            {
                var id = int.Parse(context.Request.RouteValues["id"].ToString());
                using var db = context.RequestServices.GetRequiredService<MyDatabaseContext>();
                var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id);
                if (user != null)
                {
                    await context.Response.WriteAsJsonAsync(user); // Write the response directly to the client
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            })
            .WithName("GetUserById")
            .WithOpenApi();

            group.MapPost("/", async context =>
            {
                var user = await context.Request.ReadFromJsonAsync<User>();
                using var db = context.RequestServices.GetRequiredService<MyDatabaseContext>();
                db.Users.Add(user);
                await db.SaveChangesAsync();
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsJsonAsync(new { message = "User created successfully." });
            })
            .WithName("CreateUser")
            .WithOpenApi();

            group.MapPut("/{id}", async context =>
            {
                var id = int.Parse(context.Request.RouteValues["id"].ToString());
                var user = await context.Request.ReadFromJsonAsync<User>();
                using var db = context.RequestServices.GetRequiredService<MyDatabaseContext>();
                var existingUser = await db.Users.FindAsync(id);
                if (existingUser != null)
                {
                    existingUser.Email = user.Email;
                    existingUser.Password = user.Password;
                    existingUser.Username = user.Username;
                    existingUser.PurchaseHistory = user.PurchaseHistory;
                    existingUser.ShippingAddress = user.ShippingAddress;

                    await db.SaveChangesAsync();
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsJsonAsync(new { message = "User updated successfully." });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            })
            .WithName("UpdateUser")
            .WithOpenApi();

            group.MapDelete("/{id}", async context =>
            {
                var id = int.Parse(context.Request.RouteValues["id"].ToString());
                using var db = context.RequestServices.GetRequiredService<MyDatabaseContext>();
                var user = await db.Users.FindAsync(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsJsonAsync(new { message = "User deleted successfully." });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            })
            .WithName("DeleteUser")
            .WithOpenApi();
        }
    }
}
