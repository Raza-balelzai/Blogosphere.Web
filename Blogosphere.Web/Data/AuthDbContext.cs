using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogosphere.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            var adminRoleId = "9c3d4884-f326-45a3-9f3e-dfbe08aae739";
            var superAdminRoleId = "9c3d4884-f326-45a3-9f3e-dfbe08aae937";
            var userRoleId = "9c3d4884-f326-45a3-9f3e-dfbe08aae379";
            
            //Seed roles (Admin,Super Admin And User)
            var role = new List<IdentityRole> { 
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "Admin",
                Id=adminRoleId,
                ConcurrencyStamp = adminRoleId,
            },
            new IdentityRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SuperAdmin",
                Id=superAdminRoleId,
                ConcurrencyStamp = superAdminRoleId,
            },
             new IdentityRole
            {
                Name = "User",
                NormalizedName = "User",
                Id=userRoleId,
                ConcurrencyStamp = userRoleId,
            }
            };
            builder.Entity<IdentityRole>().HasData(role);
            //Seed SuperAdminUser
            var superAdminId = "ag7d7SG7-f326-45a3-9f3e-dfbe08aae379";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@blogosphere.com",
                Email = "superadmin@blogosphere.com",
                NormalizedEmail = "superadmin@blogosphere.com".ToUpper(),
                NormalizedUserName = "superadmin@blogosphere.com".ToUpper(),
                Id=superAdminId,

            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "superadmin@123");
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add Roles to SuperUseAdmin
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId=adminRoleId,
                    UserId=superAdminId,

                },
                new IdentityUserRole<string>
                {
                    RoleId=superAdminRoleId,
                    UserId=superAdminId,

                },
                new IdentityUserRole<string>
                {
                    RoleId=userRoleId,
                    UserId=superAdminId,

                }

            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

        }
    }
}
