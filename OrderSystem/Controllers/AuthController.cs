using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Services;

namespace OrderSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext db, AuthService auth) : ControllerBase
{
    public record RegisterRequest(string Email, string Name, string Password);
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string Token, string Email, string Name);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || req.Password.Length < 6)
            return BadRequest(new { error = "Неверные данные" });

        if (await db.Users.AnyAsync(u => u.Email == req.Email))
            return Conflict(new { error = "Email уже занято" });

        var user = new User
        {
            Email     = req.Email.Trim(),
            Name  = req.Name.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return Ok(new AuthResponse(auth.GenerateToken(user), user.Email, user.Name));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { error = "Неверное имя пользователя или пароль" });
        return Ok(new AuthResponse(auth.GenerateToken(user), user.Email, user.Name));
    }
}