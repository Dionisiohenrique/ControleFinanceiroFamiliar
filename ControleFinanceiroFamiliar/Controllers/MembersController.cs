using ControleFinanceiroFamiliar.Application.Contracts.MemberDtos;
using ControleFinanceiroFamiliar.Application.UseCases.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroFamiliar.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly MemberService _memberService;
    public  MembersController(MemberService memberService) => _memberService = memberService;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] DateOnly? from, [FromQuery] DateOnly? to, CancellationToken ct)
    {
        var f = from ?? new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var t = to ?? f.AddMonths(1).AddDays(-1);
        var result = await _memberService.ListAsync(f, t, ct);
        return Ok(result.Value);
    }

    [HttpPost("Invite")]
    public async Task<IActionResult> Invite(InviteMemberRequest req, CancellationToken ct)
    {
        var result = await _memberService.InviteAsync(req, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new {errors = result.Errors});
    }
}