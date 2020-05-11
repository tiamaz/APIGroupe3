using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Projet.API.Model
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> userRole { get; set; }
    }
}