using Microsoft.EntityFrameworkCore;

namespace NovelQT.Infra.Data.Context
{
    public class ApplicationDbContextFactory
    {
        private readonly DbContextOptions<ApplicationDbContext> options;

        public ApplicationDbContextFactory(DbContextOptions<ApplicationDbContext> options)
        {
            this.options = options;
        }

        public ApplicationDbContext Create()
        {
            return new ApplicationDbContext(options);
        }
    }
}
