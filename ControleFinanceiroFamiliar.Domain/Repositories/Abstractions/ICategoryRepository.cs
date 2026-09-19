using ControleFinanceiroFamiliar.Domain.Entities;

namespace ControleFinanceiroFamiliar.Domain.Repositories.Abstractions
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyList<Category>> GetByFamilyAsync(Guid familyId, CancellationToken ct = default);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Category category, CancellationToken ct = default);
    }
}
