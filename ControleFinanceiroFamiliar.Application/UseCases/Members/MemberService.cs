using ControleFinanceiroFamiliar.Application.Abstractions;
using ControleFinanceiroFamiliar.Application.Contracts.MemberDtos;
using ControleFinanceiroFamiliar.Domain.Common;
using ControleFinanceiroFamiliar.Domain.Entities;
using ControleFinanceiroFamiliar.Domain.Enums;
using ControleFinanceiroFamiliar.Domain.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.UseCases.Members
{
    public sealed class MemberService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUserRepository userRepository, ITransactionRepository transactionRepository, IPasswordHasher passwordHasher, ICurrentUser currentUser, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _passwordHasher = passwordHasher;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<MemberDto>>> ListAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken) 
        {
            var members = await _userRepository.GetByFamilyAsync(_currentUser.FamilyId, cancellationToken);
            var report = await _transactionRepository.GetMemberReportAsync(_currentUser.FamilyId, from, to, cancellationToken);

            var map = report.ToDictionary(r => r.MemberId);

            var list = members.Select(m => new MemberDto(m.Id, m.Name, m.Email, m.Role.ToString(), 
                m.IsActive, map.TryGetValue(m.Id, out var memberReport) ? memberReport.Expense : 0m)).ToList();
            return Result<IReadOnlyList<MemberDto>>.Ok(list);
        }

        public async Task<Result<MemberDto>> InviteAsync(InviteMemberRequest request, CancellationToken ct) 
        {
            if(_currentUser.Role != nameof(UserRole.Owner))
                return Result<MemberDto>.Fail("Only the owner can invite members.");

            var exists = await _userRepository.GetByEmailAsync(request.Email, ct);
            if(exists is not null)
                return Result<MemberDto>.Fail("A member with this email already exists.");

            var user = User.Create(request.Name, request.Email, _passwordHasher.Hash(request.Password), _currentUser.FamilyId, UserRole.Member);
            await _userRepository.AddAsync(user, ct);
            await _unitOfWork.CommitAsync(ct);

            return Result<MemberDto>.Ok(new MemberDto(user.Id, user.Name, user.Email, user.Role.ToString(), user.IsActive, 0m));
        }
    }
}
