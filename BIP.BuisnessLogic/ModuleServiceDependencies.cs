using BIP.BuisnessLogic.Services;
using BIP.DataAccess.Interfaces.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIP.BuisnessLogic
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependendcies(this IServiceCollection services)
        {

            services.AddTransient<IUserRepository, UserRepository>();
            

            return services;
        }
    }

}
