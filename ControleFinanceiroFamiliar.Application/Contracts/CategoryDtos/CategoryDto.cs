using ControleFinanceiroFamiliar.Domain.Enums;

namespace ControleFinanceiroFamiliar.Application.Contracts.CategoryDtos
{
    public record CategoryDto(Guid Id, string Name, TransactionType Type, string Color, string? Icon);
}
