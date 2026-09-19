using ControleFinanceiroFamiliar.Domain.Enums;

namespace ControleFinanceiroFamiliar.Application.Contracts.CategoryDtos
{
    public record CreateCategoryRequest(string Name, TransactionType Type, string Color, string? Icon);
}
