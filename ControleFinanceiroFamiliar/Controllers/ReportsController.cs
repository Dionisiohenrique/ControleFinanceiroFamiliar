using ControleFinanceiroFamiliar.Application.UseCases.Reports;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroFamiliar.Api.Controllers
{
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("by-member")]
        public async Task<IActionResult> ByMember([FromQuery] DateOnly from, DateOnly to,
            CancellationToken ct) => Ok((await _reportService.ByMemberAsync(from, to, ct)).Value);

        [HttpGet("by-category")]
        public async Task<IActionResult> ByCategory([FromQuery] DateOnly from, [FromQuery] DateOnly to,
            CancellationToken ct) => Ok((await _reportService.ByCategoryAsync(from, to, ct)).Value);

        [HttpGet("summary")]
        public async Task<IActionResult> Summary([FromQuery] DateOnly from, [FromQuery] DateOnly to,
            CancellationToken ct) => Ok((await _reportService.GetSummaryAsync(from, to, ct)).Value);
    }
}