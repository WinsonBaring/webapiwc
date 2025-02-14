using webapiwc.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace webapiwc.Endpoints;

public static class ApplicationUserEndpoints
{
    public static RouteGroupBuilder MapApplicationUserEndpoint(this WebApplication app)
    {
        var applicationUserGroup = app.MapGroup("/api/applicationuser");

        // Get all users
        applicationUserGroup.MapGet("/", async (AppDbContext db) =>
        {
            return await db.ApplicationUser.ToListAsync();
        });

        // Register User
        applicationUserGroup.MapPost("/register", async (RegisterRequest request, UserManager<ApplicationUser> userManager) =>
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(new { Message = "User registered successfully!" });
        });

        // Login User (JWT Authentication)
        applicationUserGroup.MapPost("/login", async (LoginRequest request, UserManager<ApplicationUser> userManager, IConfiguration config) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                Console.WriteLine("User not found or password is incorrect");
                Console.WriteLine("request body: " + request.Email + " " + request.Password);
                
                return Results.Unauthorized();
            }

            var token = GenerateJwtToken(user, config);
            Console.WriteLine("Token generated");
            return Results.Ok(new { Token = token });
        });

        return applicationUserGroup;
    }

    // Generate JWT Token
    private static string GenerateJwtToken(ApplicationUser user, IConfiguration config)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("FullName", user.FullName),
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

// Request DTOs
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
