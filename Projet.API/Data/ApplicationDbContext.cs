using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projet.API.Model;

namespace Projet.API.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating (ModelBuilder builder)
        {
                base.OnModelCreating(builder);

                builder.Entity<EtudiantClasse>().HasKey(k => new {k.ClasseId, k.UserId});

                builder.Entity<UserRole>(
                    userRole => {
                        userRole.HasKey(k => new {k.UserId, k.RoleId});
                    }

                    

                );
        }


    }
}