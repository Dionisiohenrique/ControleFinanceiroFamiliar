using ControleFinanceiroFamiliar.Domain.Entities;
using ControleFinanceiroFamiliar.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroFamiliar.Infra.Repositories
{
    public class FamilyRepository
    {
        private readonly AppDbContext _db;
        public FamilyRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<Family?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => _db.Families.Include(f => f.Members).FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        public async Task AddAsync(Family f, CancellationToken ct = default) => await _db.Families.AddAsync(f, ct);
    }
}
