using ControleFinanceiroFamiliar.Domain.Common;
using ControleFinanceiroFamiliar.Domain.Enums;


namespace ControleFinanceiroFamiliar.Domain.Entities;

public class Category : Entity
{
    public string Name { get; private set; } = default!;
    public string Color { get; private set; } = "#4F46E5";
    public string? Icon { get; private set; }
    public TransactionType Type { get; private set; }
    public Guid FamilyId { get; private set; }

    private Category() { }

    public static Category Create(string name, TransactionType type, Guid familyId,
        string color = "#4F46E5", string? icon = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nome da categoria é obrigatório");
        return new Category { Name = name.Trim(), Type = type, FamilyId = familyId, Color = color, Icon = icon };
    }
}