using CheckIn.Frontend.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Helper
{

    public static class Instantiate
    {
        /// <summary>
        /// Adds all our api services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection addServices(this IServiceCollection services)
        {
            services.AddScoped<ICheckTimes, CheckTimes>();
            services.AddScoped<IRooms, Rooms>();
            services.AddScoped<IScanners, Scanners>();
            services.AddScoped<IUsers, Users>();
            services.AddScoped<IYearSummeries, YearSummeries>();
            services.AddScoped<IServices, CollectionOfServices>();
            return services;
        }
    }
}
