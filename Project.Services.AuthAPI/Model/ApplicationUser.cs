using Microsoft.AspNetCore.Identity;

namespace Project.Services.AuthAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
