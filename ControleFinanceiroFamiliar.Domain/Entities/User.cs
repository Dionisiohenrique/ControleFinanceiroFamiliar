using ControleFinanceiroFamiliar.Domain.Common;
using ControleFinanceiroFamiliar.Domain.Enums;

namespace ControleFinanceiroFamiliar.Domain.Entities;

public class User : Entity
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public Guid FamilyId { get; private set; }
    public Family Family { get; private set; } = default!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private User() { }

    public static User Create(string name, string email, string passwordHash, Guid familyId, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nome é obrigatório");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email inválido");

        return new User
        {
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            FamilyId = familyId,
            Role = role
        };
    }

    public void UpdatePassword(string hash) => PasswordHash = hash;
    public void Deactivate() => IsActive = false;
}