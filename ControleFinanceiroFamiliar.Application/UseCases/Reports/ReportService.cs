using ControleFinanceiroFamiliar.Application.Abstractions;
using ControleFinanceiroFamiliar.Application.Contracts.ReportDtos;
using ControleFinanceiroFamiliar.Domain.Common;
using ControleFinanceiroFamiliar.Domain.Enums;
using ControleFinanceiroFamiliar.Domain.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace ControleFinanceiroFamiliar.Application.UseCases.Reports
{
    public sealed class ReportService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUser _currentUser;

        public ReportService(ITransactionRepository transactionRepository, ICurrentUser currentUser)
        {
            _transactionRepository = transactionRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<IReadOnlyList<MemberReportDto>>> ByMemberAsync(DateOnly from, DateOnly to, CancellationToken ct)
        {
            var data = await _transactionRepository.GetMemberReportAsync(_currentUser.FamilyId, from, to, ct);
            var result = data.Select(d => new MemberReportDto(d.MemberId, d.MemberName, d.Income, d.Expense, d.Income - d.Expense)).ToList();

            return Result<IReadOnlyList<MemberReportDto>>.Ok(result);
        }

        public async Task<Result<IReadOnlyList<CategoryReportDto>>> ByCategoryAsync(DateOnly from, DateOnly to, CancellationToken ct)
        {
            var data = await _transactionRepository.GetCategoryReportAsync(_currentUser.FamilyId, from, to, ct);
            var result = data.Select(d => new CategoryReportDto(d.CategoryId, d.CategoryName, d.Color, d.Type, d.Total)).ToList();

            return Result<IReadOnlyList<CategoryReportDto>>.Ok(result);
        }

        public async Task<Result<SummaryDto>> GetSummaryAsync(DateOnly from, DateOnly to, CancellationToken ct)
        {
            var txs = await _transactionRepository.GetByFamilyAsync(_currentUser.FamilyId, from, to, null, ct);
            var income = txs.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var expense = txs.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

            return Result<SummaryDto>.Ok(new SummaryDto(income, expense, income - expense, txs.Count()));
        }
    }
}
