using ControleFinanceiroFamiliar.Domain.Repositories.Abstractions;
using ControleFinanceiroFamiliar.Infra.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public async Task<int> CommitAsync(CancellationToken ct = default)
            => await _db.SaveChangesAsync(ct);
    }
}
