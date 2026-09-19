using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.Contracts.ReportDtos
{
    public record SummaryDto(decimal TotalIncome, decimal TotalExpense, decimal Balance, int TransactionCount);
}
