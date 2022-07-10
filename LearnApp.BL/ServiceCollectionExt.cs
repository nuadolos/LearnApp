using LearnApp.BL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BL
{
    static public class ServiceCollectionExt
    {
        public static void AddBLService(this IServiceCollection services)
        {
            services.AddTransient<AccountService>();
        }
    }
}
