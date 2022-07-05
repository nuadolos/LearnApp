using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Context
{
    public class LearnContextFactory : IDesignTimeDbContextFactory<LearnContext>
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
