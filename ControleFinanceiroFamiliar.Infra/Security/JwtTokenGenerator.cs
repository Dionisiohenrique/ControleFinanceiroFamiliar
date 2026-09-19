using ControleFinanceiroFamiliar.Application.Abstractions;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ControleFinanceiroFamiliar.Infra.Security
{
    public class JwtOptions
    {
        public string Secret { get; set; } = default;
        public string Issuer { get; set; } = "FamilyFinance";
        public string Audience { get; set; } = "FamilyFinance.Client";
        public int ExpiresInMinutes { get; set; } = 400;
    }

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _opts;
        public JwtTokenGenerator(IOptions<JwtOptions> opts) => _opts = opts.Value;
        public (string token, DateTime expiresAt) Generate(Guid userId, Guid familyId, string name, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_opts.ExpiresInMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("familyId", familyId.ToString()),
                new Claim("name", name)
            };

            var token = new JwtSecurityToken(_opts.Issuer, _opts.Audience, claims, expires: expires, signingCredentials: creds);
            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
