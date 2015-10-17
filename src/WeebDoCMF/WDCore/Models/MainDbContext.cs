using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using WeebDoCMF.WDCore.Models.Translations;

namespace WeebDoCMF.WDCore.Models
{
    public class MainDbContext : IdentityDbContext<WeebDoCmfUser>
    {
        public DbSet<TCulture> TCultures { get; set; }
        public DbSet<TResource> TResources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TCulture>()
                .Property(c => c.CultureCode)
                .HasMaxLength(5);
        }
    }
}
