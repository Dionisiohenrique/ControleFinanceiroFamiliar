using ControleFinanceiroFamiliar.Domain.Common;

namespace ControleFinanceiroFamiliar.Domain.Entities;

public class Family : Entity
{
    public string Name { get; private set; } = default!;
    public string Currency { get; private set; } = "BRL";
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private readonly List<User> _members = new();
    public IReadOnlyCollection<User> Members => _members;

    private Family() { }

    public static Family Create(string name, string currency = "BRL")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da família é obrigatório");
        return new Family { Name = name.Trim(), Currency = currency };
    }
}