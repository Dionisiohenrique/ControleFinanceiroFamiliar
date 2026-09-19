using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace ControleFinanceiroFamiliar.Infra.Services
{
    public class CurrentUser
    {
        private readonly IHttpContextAccessor _acessor;
        public CurrentUser(IHttpContextAccessor acessor) => _acessor = acessor;

        private ClaimsPrincipal User => _acessor.HttpContext?.User ?? throw new UnauthorizedAccessException();

        public Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")!);

        public Guid FamilyId => Guid.TryParse();
    }
}
