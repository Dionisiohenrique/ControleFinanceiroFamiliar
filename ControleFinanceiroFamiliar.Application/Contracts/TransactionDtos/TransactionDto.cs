using ControleFinanceiroFamiliar.Domain.Enums;

namespace ControleFinanceiroFamiliar.Application.Contracts.TransactionDtos;

public record TransactionDto(Guid Id, decimal Amount, TransactionType Type, Guid CategoryId, string CategoryName,
    string CategoryColor, Guid MemberId, string MemberName, DateOnly Date, string? Description);

