using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ControleFinanceiroFamiliar.Application.Abstractions;


namespace ControleFinanceiroFamiliar.Infra.Services
{
    public class CurrentUser(IHttpContextAccessor acessor) : ICurrentUser
    {
        private ClaimsPrincipal User => acessor.HttpContext?.User ?? throw new UnauthorizedAccessException();

        public Guid UserId
        {
            get
            {
                var text = User.FindFirst(ClaimTypes.NameIdentifier);
                if (text == null)
                {
                    text = User.FindFirst("sub")!;
                }

                return Guid.Parse(text.Value);
            }
        }

        public Guid FamilyId
        {
            get
            {
                var text = User.FindFirst("familyId")!;
                return Guid.Parse(text.Value);
            }
        }

        public string Name => User.FindFirst("name")?.Value ?? "";
        public string Role => User.FindFirst(ClaimTypes.Role)?.Value ?? "";
    }
}
