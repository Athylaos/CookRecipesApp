using BCrypt.Net;
using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace CookRecipesApp.API.Endpoints
{
    public static class UserEndpoint
    {

        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/users");


            // UserRegistration
            group.MapPost("/register", async (UserRegistrationDto registrationDto, CookRecipesDbContext db) =>
            {
                if (await db.Users.AnyAsync(u => u.Email == registrationDto.Email))
                {
                    return Results.BadRequest("Email already in use");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = registrationDto.Email,
                    PasswordHash = passwordHash,
                    Name = registrationDto.Name,
                    Surname = registrationDto.Surname,
                    UserCreated = DateTime.UtcNow,
                    Role = "user"
                };

                db.Users.Add(newUser);
                await db.SaveChangesAsync();

                return Results.Ok(new { newUser.Id, newUser.Email });
            });


            //UserLogin
            group.MapPost("/login", async (UserLoginDto loginDto, CookRecipesDbContext db) =>
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null)
                {
                    return Results.Unauthorized();
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(new
                {
                    user.Id,
                    user.Email,
                    user.Name,
                    user.Surname,
                    user.Role
                });
            });







        }




    }
}
