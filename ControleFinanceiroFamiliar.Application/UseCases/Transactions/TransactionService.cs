using ControleFinanceiroFamiliar.Application.Abstractions;
using ControleFinanceiroFamiliar.Application.Contracts.TransactionDtos;
using ControleFinanceiroFamiliar.Domain.Common;
using ControleFinanceiroFamiliar.Domain.Entities;
using ControleFinanceiroFamiliar.Domain.Repositories.Abstractions;

namespace ControleFinanceiroFamiliar.Application.UseCases.Transactions
{
    public sealed class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<IReadOnlyList<TransactionDto>>> ListAsync(DateOnly? from, DateOnly? to, Guid? memberId, CancellationToken cancellationToken)
        {
            var items = await _transactionRepository.GetByFamilyAsync(_currentUser.FamilyId, from, to, memberId, cancellationToken);
            return Result<IReadOnlyList<TransactionDto>>.Ok(items.Select(TransactionMap).ToList());
        }

        public async Task<Result<TransactionDto>> CreateAsync(CreateTransactionRequest req, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(req.CategoryId, ct);
            if (category is null || !category.Equals(_currentUser.FamilyId))
                return Result<TransactionDto>.Fail("Categoria não encontrada.");

            var transaction = Transaction.Create(req.Amount, req.Type, req.CategoryId, _currentUser.UserId, _currentUser.FamilyId, req.Date, req.Description);

            await _transactionRepository.AddAsync(transaction, ct);
            await _unitOfWork.CommitAsync(ct);

            var enriched = await _transactionRepository.GetByIdAsync(transaction.Id, ct);
            return Result<TransactionDto>.Ok(TransactionMap(enriched!));
        }


        public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id, ct);
            if (transaction.MemberId != _currentUser.UserId && _currentUser.Role != "Owner")
                return Result.Fail("Transação não encontrada.");
            _transactionRepository.Remove(transaction);
            await _unitOfWork.CommitAsync(ct);
            return Result.Ok();
        }

        private static TransactionDto TransactionMap(Transaction t) =>
         new TransactionDto(t.Id, t.Amount, t.Type, t.CategoryId, t.Category?.Name ?? "",
             t.Category?.Color ?? "#4F46E5", t.MemberId, t.Member?.Name ?? "",
             t.Date, t.Description);
    }
}
