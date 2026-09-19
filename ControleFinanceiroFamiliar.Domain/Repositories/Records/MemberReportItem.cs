namespace ControleFinanceiroFamiliar.Domain.Repositories.Records
{
    public record MemberReportItem(Guid MemberId, string MemberName, decimal Income, decimal Expense);
}
