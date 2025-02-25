using Microsoft.AspNetCore.Identity;

namespace Testiculo.Domain.Identity
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}