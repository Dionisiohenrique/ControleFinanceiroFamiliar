using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.Abstractions
{
    public interface IJwtTokenGenerator
    {
        (string token, DateTime expiresAt) Generate(Guid userId, Guid familyId, string name, string role);
    }
}
