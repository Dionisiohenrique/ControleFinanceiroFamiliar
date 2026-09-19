using ControleFinanceiroFamiliar.Domain.Entities;
using ControleFinanceiroFamiliar.Domain.Repositories.Abstractions;
using ControleFinanceiroFamiliar.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroFamiliar.Infra.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Category category, CancellationToken ct = default)
            => await _db.Categories.AddAsync(category, ct);

        public async Task<IReadOnlyList<Category>> GetByFamilyAsync(Guid familyId, CancellationToken ct = default)
            => await _db.Categories.Where(x => x.FamilyId == familyId).OrderBy(c => c.Name).ToListAsync(ct);

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _db.Categories.FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
