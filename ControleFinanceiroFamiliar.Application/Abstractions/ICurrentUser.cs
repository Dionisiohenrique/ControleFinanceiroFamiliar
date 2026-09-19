using System;
using System.Collections.Generic;
using System.Text;

namespace ControleFinanceiroFamiliar.Application.Abstractions
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        Guid FamilyId { get; }
        string Name { get; }
        string Role { get; }
    }
}
