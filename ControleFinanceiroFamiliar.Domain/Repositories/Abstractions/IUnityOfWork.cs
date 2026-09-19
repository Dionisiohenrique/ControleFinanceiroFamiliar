namespace ControleFinanceiroFamiliar.Domain.Repositories.Abstractions
{
public interface IUnitOfWork { 
        Task<int> CommitAsync(CancellationToken ct = default); }
}
