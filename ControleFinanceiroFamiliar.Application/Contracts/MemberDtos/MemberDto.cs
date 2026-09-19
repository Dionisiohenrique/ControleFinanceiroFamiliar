using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.Contracts.MemberDtos
{
    public record MemberDto(Guid Id, string Name, string Email, string Role, bool IsActive, decimal MonthExpense);
}
