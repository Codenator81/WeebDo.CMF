using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace WeebDoCMF.Core.Models
{
    public class MainDbContext : IdentityDbContext<WeebDoCmsUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
