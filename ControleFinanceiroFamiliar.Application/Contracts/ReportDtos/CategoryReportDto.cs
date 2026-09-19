using ControleFinanceiroFamiliar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.Contracts.ReportDtos
{
    public record CategoryReportDto(Guid CategoryId, string CategoryName, string Color, TransactionType Type, decimal Total);
}
