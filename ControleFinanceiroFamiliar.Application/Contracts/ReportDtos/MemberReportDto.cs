using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.Contracts.ReportDtos
{
    public record MemberReportDto(Guid MemberId, string MemberName, decimal Income, decimal Expense, decimal Balance);
}
