using LearnApp.DAL.Context;
using LearnApp.DAL.Repos;
using LearnApp.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL
{
    static public class ServiceCollectionExt
    {
        public static void AddDALService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<LearnContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("LearnConnection")));

            services.AddScoped<ILearnRepo, LearnRepo>();
            services.AddScoped<IShareNoteRepo, ShareNoteRepo>();
            services.AddScoped<IGroupRepo, GroupRepo>();
            services.AddScoped<IFollowerRepo, FollowerRepo>();
            services.AddScoped<INoteRepo, NoteRepo>();
            services.AddScoped<IGroupUserRepo, GroupUserRepo>();
            services.AddScoped<ILearnDocRepo, LearnDocRepo>();
            services.AddScoped<IAttachRepo, AttachRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
        }
    }
}
