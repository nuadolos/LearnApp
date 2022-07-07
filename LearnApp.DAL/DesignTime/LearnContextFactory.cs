using LearnApp.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LearnApp.DAL.DesignTime
{
    public class LearnContextDesignTime : IDesignTimeDbContextFactory<LearnContext>
    {
        public LearnContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LearnContext>();

            string connectionString = @"server=.\sqlexpress;database=KnowledgeLibrary;
                    integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

            optionsBuilder.UseSqlServer(connectionString,
                options => options.EnableRetryOnFailure());

            return new LearnContext(optionsBuilder.Options);
        }
    }
}

// dotnet ef migrations add LearnDbMigration -c LearnContext (--context LearnApp.DAL.Context.LearnContext -o Migrations)
// dotnet ef database update -c LearnContext
// dotnet ef dbcontext scaffold @connectionString ("server=...; App=EntityFramework") Microsoft.EntityFrameworkCore.SqlServer -o Entities