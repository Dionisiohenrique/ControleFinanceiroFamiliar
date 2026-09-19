using ControleFinanceiroFamiliar.Domain.Repositories.Records;
using ControleFinanceiroFamiliar.Domain.Entities;

namespace ControleFinanceiroFamiliar.Domain.Repositories.Abstractions
{
    public interface ITransactionRepository
    {
        Task<IReadOnlyList<Transaction>> GetByFamilyAsync(Guid familyId, DateOnly? from, DateOnly? to, Guid? memberId, CancellationToken ct = default);
        Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Transaction t, CancellationToken ct = default);
        void Remove(Transaction t);
        Task<IReadOnlyList<MemberReportItem>> GetMemberReportAsync(Guid familyId, DateOnly from, DateOnly to, CancellationToken ct = default);
        Task<IReadOnlyList<CategoryReportItem>> GetCategoryReportAsync(Guid familyId, DateOnly from, DateOnly to, CancellationToken ct = default);
    }
}
