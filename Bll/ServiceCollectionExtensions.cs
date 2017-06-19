using Microsoft.Extensions.DependencyInjection;

namespace Marigold
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBll(this IServiceCollection services)
        {
            services.AddScoped<IServicesBll,ServicesBll>();
            services.AddScoped<IReservationsBll,ReservationsBll>();
        }
    }
}