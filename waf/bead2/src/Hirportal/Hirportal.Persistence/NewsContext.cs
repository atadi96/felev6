
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hirportal.Persistence
{
    public class NewsContext : IdentityDbContext<Author, IdentityRole<int>, int>
    {
        public NewsContext(DbContextOptions<NewsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Author>().ToTable("Authors");
            builder.Entity<Article>()
                .Property(a => a.Modified)
                .HasDefaultValueSql("getdate()");
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleImage> Images { get; set; }
    }
}
