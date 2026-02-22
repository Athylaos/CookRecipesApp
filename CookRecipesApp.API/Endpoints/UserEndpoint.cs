using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            group.MapPost("/login", async (UserLoginDto loginDto, CookRecipesDbContext db, IConfiguration config) =>
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return Results.Unauthorized();
                }

                var token = GenerateJwtToken(user, config);

                return Results.Ok(new LoginResponse
                {
                    Token = token,
                    User = new User()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        Surname = user.Surname,
                        UserCreated = user.UserCreated,
                        Role = user.Role,
                        AvatarUrl = user.AvatarUrl,                        
                    }
                });
            });


            //GetMe
            group.MapGet("/getMe", async (ClaimsPrincipal user, CookRecipesDbContext db) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var userId = Guid.Parse(userIdClaim);

                var userData = await db.Users.AsNoTracking().Where(u => u.Id == userId).Select(u => new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = u.Name,
                    Surname = u.Surname,
                    Role = u.Role,
                    AvatarUrl = u.AvatarUrl ?? "default_avatar.png",
                    UserCreated = u.UserCreated
                }).FirstOrDefaultAsync();

                return Results.Ok(userData);
            }).RequireAuthorization();


            //GetUserDisplay
            group.MapGet("/getUserDisplay/{userId:guid}", async (Guid userId,CookRecipesDbContext db) =>
            {

                var userData = await db.Users.AsNoTracking().Where(u => u.Id == userId).Select(u => new UserDisplayDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    AvatarUrl = u.AvatarUrl ?? "default_avatar.png",
                    UserCreated = u.UserCreated
                }).FirstOrDefaultAsync();

                return Results.Ok(userData);
            });






        }

        
        private static string GenerateJwtToken(User user, IConfiguration config)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyString = config["JWTKey:Default"];

            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key is missing in appsettings.json");

            var key = Encoding.ASCII.GetBytes(keyString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role ?? "user")
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}