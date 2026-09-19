using ControleFinanceiroFamiliar.Domain.Entities;

namespace ControleFinanceiroFamiliar.Domain.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<User>> GetByFamilyAsync(Guid familyId, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
    }
}
