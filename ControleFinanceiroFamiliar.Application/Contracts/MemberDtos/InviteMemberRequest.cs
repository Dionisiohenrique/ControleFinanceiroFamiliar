using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.Contracts.MemberDtos
{
    public record InviteMemberRequest(string Name, string Email, string Password);
}
