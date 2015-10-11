using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace WeebDoCMF.WDCore.Models
{
    public class MainDbContext : IdentityDbContext<WeebDoCmfUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
