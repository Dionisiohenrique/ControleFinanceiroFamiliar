using ControleFinanceiroFamiliar.Domain.Enums;

namespace ControleFinanceiroFamiliar.Domain.Repositories.Records
{
    public record CategoryReportItem(Guid CategoryId, string CategoryName, string Color, TransactionType Type, decimal Total);
}
