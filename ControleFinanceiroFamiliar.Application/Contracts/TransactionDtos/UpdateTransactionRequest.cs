using ControleFinanceiroFamiliar.Domain.Enums;

namespace ControleFinanceiroFamiliar.Application.Contracts.TransactionDtos
{
    public record UpdateTransactionRequest(decimal Amount, TransactionType Type, Guid CategoryId, DateOnly Date, string? Description);
}
