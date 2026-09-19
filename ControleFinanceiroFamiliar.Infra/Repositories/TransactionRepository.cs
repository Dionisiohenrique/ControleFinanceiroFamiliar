using ControleFinanceiroFamiliar.Domain.Entities;
using ControleFinanceiroFamiliar.Domain.Enums;
using ControleFinanceiroFamiliar.Domain.Repositories.Abstractions;
using ControleFinanceiroFamiliar.Domain.Repositories.Records;
using ControleFinanceiroFamiliar.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroFamiliar.Infra.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;

        public TransactionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Transaction t, CancellationToken ct = default)
            => await _db.Transactions.AddAsync(t, ct);

        public async Task<IReadOnlyList<Transaction>> GetByFamilyAsync(Guid familyId, DateOnly? from, DateOnly? to, Guid? memberId, CancellationToken ct = default)
        {
            var q = _db.Transactions.Include(c => c.Category).Include(t => t.Member).AsQueryable();
            q = q.Where(t => t.FamilyId == familyId);
            if (from.HasValue) q = q.Where(t => t.Date >= from.Value);
            if (to.HasValue) q = q.Where(t => t.Date <= to.Value);
            if (memberId.HasValue) q = q.Where(t => t.MemberId == memberId.Value);
            return await q.OrderByDescending(t => t.Date).ThenByDescending(t => t.CreatedAt).ToListAsync(ct);
        }

        public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.Transactions.Include(t => t.Category).Include(t => t.Member).FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<IReadOnlyList<CategoryReportItem>> GetCategoryReportAsync(Guid familyId, DateOnly from, DateOnly to, CancellationToken ct = default)
            => await _db.Transactions
            .Where(t => t.FamilyId == familyId && t.Date >= from && t.Date <= to)
            .GroupBy(t => new { t.CategoryId, t.Category.Name, t.Category.Color, t.Type })
            .Select(t => new CategoryReportItem(t.Key.CategoryId, t.Key.Name, t.Key.Color, t.Key.Type, t.Sum(x => x.Amount))).ToListAsync(ct);

        public async Task<IReadOnlyList<MemberReportItem>> GetMemberReportAsync(Guid familyId, DateOnly from, DateOnly to, CancellationToken ct = default)
            => await _db.Transactions
                .Where(t => t.FamilyId == familyId && t.Date >= from && t.Date <= to)
                .GroupBy(t => new { t.MemberId, t.Member.Name })
                .Select(g => new MemberReportItem(
                    g.Key.MemberId,
                    g.Key.Name,
                    g.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount),
                    g.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount)))
                .ToListAsync(ct);

        public void Remove(Transaction t)
            => _db.Transactions.Remove(t);
    }
}
