using ControleFinanceiroFamiliar.Domain.Common;
using ControleFinanceiroFamiliar.Domain.Enums;

namespace ControleFinanceiroFamiliar.Domain.Entities;

public class Transaction : Entity
{
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = default!;
    public Guid MemberId { get; private set; }
    public User Member { get; private set; } = default!;
    public Guid FamilyId { get; private set; }
    public DateOnly Date { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Transaction() { }

    public static Transaction Create(decimal amount, TransactionType type, Guid categoryId,
        Guid memberId, Guid familyId, DateOnly date, string? description)
    {
        if (amount <= 0) throw new ArgumentException("Valor deve ser maior que zero");
        return new Transaction
        {
            Amount = amount,
            Type = type,
            CategoryId = categoryId,
            MemberId = memberId,
            FamilyId = familyId,
            Date = date,
            Description = description?.Trim()
        };
    }

    public void Update(decimal amount, TransactionType type, Guid categoryId, DateOnly date, string? description)
    {
        if (amount <= 0) throw new ArgumentException("Valor deve ser maior que zero");
        Amount = amount; Type = type; CategoryId = categoryId; Date = date; Description = description?.Trim();
    }
}