using ControleFinanceiroFamiliar.Domain.Enums;

namespace ControleFinanceiroFamiliar.Application.Contracts.TransactionDtos
{
    public record CreateTransactionRequest(decimal Amount, TransactionType Type, Guid CategoryId, DateOnly Date, string? Description);
}
