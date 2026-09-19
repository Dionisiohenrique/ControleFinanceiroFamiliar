namespace ControleFinanceiroFamiliar.Application.Contracts.AuthDtos
{
    public record AuthResponse(string Token, DateTime ExpiresAt, UserDto User);
}
