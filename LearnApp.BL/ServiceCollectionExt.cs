using LearnApp.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.BLL
{
    static public class ServiceCollectionExt
    {
        public static void AddBLService(this IServiceCollection services)
        {
            services.AddTransient<AccountService>();
        }
    }
}
