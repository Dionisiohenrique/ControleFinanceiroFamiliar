using ControleFinanceiroFamiliar.Domain.Entities;
using ControleFinanceiroFamiliar.Domain.Repositories.Abstractions;
using ControleFinanceiroFamiliar.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroFamiliar.Infra.Repositories
{


    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(User user, CancellationToken ct = default)
            => await _db.Users.AddAsync(user, ct);
        
        public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
            => _db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

        public Task<List<User>> GetByFamilyAsync(Guid familyId, CancellationToken ct = default)
            => _db.Users.Where(u => u.FamilyId == familyId).OrderBy(u => u.Name).ToListAsync(ct);

        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }
}
