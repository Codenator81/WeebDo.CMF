using Microsoft.AspNet.Identity.EntityFramework;

namespace WeebDoCMF.WDCore.Models
{
    public class WeebDoCmfUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    }
}
