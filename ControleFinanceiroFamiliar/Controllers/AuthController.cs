using ControleFinanceiroFamiliar.Application.Contracts.AuthDtos;
using ControleFinanceiroFamiliar.Application.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroFamiliar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService) =>  _authService = authService;

    [HttpPost("register-family")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterFamilyRequest req, CancellationToken ct)
    {
        var result = await _authService.RegisterFamilyAsync(req, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(new {errors = result.Errors});
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest req, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(req, ct);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new { errors = result.Errors });
    }
}