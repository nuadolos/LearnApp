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
        public static void AddBLLService(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<FollowerService>();
            services.AddScoped<GroupService>();
            services.AddScoped<GroupUserService>();
            services.AddScoped<LearnService>();
            services.AddScoped<NoteService>();
            services.AddScoped<ShareNoteService>();
        }
    }
}
