using ControleFinanceiroFamiliar.Application.UseCases.Auth;
using ControleFinanceiroFamiliar.Application.UseCases.Members;
using ControleFinanceiroFamiliar.Application.UseCases.Reports;
using ControleFinanceiroFamiliar.Application.UseCases.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace ControleFinanceiroFamiliar.Infra.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<MemberService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<ReportService>();

            return services;
        }
    }
}
