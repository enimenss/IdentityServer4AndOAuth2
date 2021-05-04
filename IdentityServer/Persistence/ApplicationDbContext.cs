using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityServer.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityServer.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<AppUser>();


            builder.Entity<AppUser>().HasData(
                  new AppUser
                  {
                      Id = -1,
                      UserName = "administrator@localhost",
                      NormalizedUserName = "ADMINISTRATOR@LOCALHOST",
                      Email = "administrator@localhost",
                      NormalizedEmail = "ADMINISTRATOR@LOCALHOST",
                      SecurityStamp = Guid.NewGuid().ToString(),
                      PasswordHash = hasher.HashPassword(null, "Administrator!0707")
                  });
        }
    }
}
