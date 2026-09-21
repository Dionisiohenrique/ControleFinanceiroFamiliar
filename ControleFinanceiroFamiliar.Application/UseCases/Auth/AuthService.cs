using ControleFinanceiroFamiliar.Application.Abstractions;
using ControleFinanceiroFamiliar.Application.Contracts.AuthDtos;
using ControleFinanceiroFamiliar.Domain.Common;
using ControleFinanceiroFamiliar.Domain.Entities;
using ControleFinanceiroFamiliar.Domain.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.UseCases.Auth
{
    public sealed class AuthService
    {
        private readonly IFamilyRepository _familyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IFamilyRepository familyRepository, IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
        {
            _familyRepository = familyRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthResponse>> RegisterFamilyAsync(RegisterFamilyRequest request, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if(existing is not null) return Result<AuthResponse>.Fail("Email already in use.");

            var family = Family.Create(request.FamilyName);
            await _familyRepository.AddAsync(family, cancellationToken);
            var user= User.Create(request.OwnerName, request.Email, _passwordHasher.Hash(request.Password), family.Id, Domain.Enums.UserRole.Owner);
            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            var (token, exp) = _jwtTokenGenerator.Generate(user.Id, user.FamilyId, user.Name, user.Role.ToString());
            return Result<AuthResponse>.Ok(new AuthResponse(token, exp, new UserDto(user.Id, user.Name, user.Email, user.Role.ToString(), user.FamilyId)));

        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            var user= await _userRepository.GetByEmailAsync(request.Email, ct);
            if(user is null || !user.IsActive || !_passwordHasher.Verify(request.Password, user.PasswordHash))
                return Result<AuthResponse>.Fail("Invalid credentials.");
            
            var (token, exp) = _jwtTokenGenerator.Generate(user.Id, user.FamilyId, user.Name, user.Role.ToString());

            return Result<AuthResponse>.Ok(new AuthResponse(token, exp, new UserDto(user.Id, user.Name, user.Email, user.Role.ToString(), user.FamilyId)));
        } 
    }
}
