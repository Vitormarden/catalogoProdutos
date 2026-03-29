using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Catalogo_loja.DTOs;

namespace Catalogo_loja.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (model.Password != model.ConfirmPassword)
            return BadRequest(new AuthResponseDto { Success = false, Message = "As senhas não coincidem." });

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return BadRequest(new AuthResponseDto { Success = false, Message = "Este e-mail já está em uso." });

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok(new AuthResponseDto { Success = true, Message = "Usuário registrado com sucesso!" });
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return BadRequest(new AuthResponseDto { Success = false, Message = $"Erro ao registrar: {errors}" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var token = GenerateJwtToken(user);
            return Ok(new AuthResponseDto { Success = true, Token = token });
        }

        return Unauthorized(new AuthResponseDto { Success = false, Message = "Invalid credentials." });
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "chave_secreta_muito_longa_para_desenvolvimento_local";
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? user.UserName ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "CatalogoLoja",
            audience: _configuration["Jwt:Audience"] ?? "CatalogoLojaFrontend",
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}