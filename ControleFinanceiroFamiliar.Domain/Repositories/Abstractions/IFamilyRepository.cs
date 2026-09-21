using ControleFinanceiroFamiliar.Domain.Entities;

namespace ControleFinanceiroFamiliar.Domain.Repositories.Abstractions
{
    public interface IFamilyRepository
    {
        Task<Family?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Family family, CancellationToken ct = default);
    }
}
