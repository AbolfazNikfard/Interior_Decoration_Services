using Interior_Decoration_Services.Models;
using Microsoft.AspNetCore.Identity;

namespace Interior_Decoration_Services.Data
{
    public class Database_Initializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceProvider>()
                .CreateScope())
            {
                User adminUser = null;
                User serviceUser = null;
                IdentityRole adminRole = null;
                IdentityRole serviceRole = null;
                var context = serviceScope.ServiceProvider.GetRequiredService<ProjectContext>();
                if (context != null)
                {
                    if (context.Users != null && !context.Users.Any())
                    {
                        adminUser = new User()
                        {
                            PasswordHash =
                                "AQAAAAEAACcQAAAAEE5W8z3JXjlDAENV/mrcVLZ8rlmSq3FzpNfatgjigHhfrvQPEMIjQRLNUYED5Nt9rQ==",
                            Name = "negin",
                            Family = "daryadel",
                            UserName = "Admin@gmail.com",
                            NormalizedUserName = "ADMIN@GMAIL.COM",
                            Email = "Admin@gmail.com",
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            EmailConfirmed = true,
                        };
                        serviceUser = new User()
                        {
                            PasswordHash =
                            "AQAAAAEAACcQAAAAEE5W8z3JXjlDAENV/mrcVLZ8rlmSq3FzpNfatgjigHhfrvQPEMIjQRLNUYED5Nt9rQ==",
                            UserName = "Service@gmail.com",
                            NormalizedUserName = "SERVICE@GMAIL.COM",
                            Email = "Service@gmail.com",
                            NormalizedEmail = "SERVICE@GMAIL.COM",
                            EmailConfirmed = true,
                        };
                        context.Users.Add(adminUser);
                        context.Users.Add(serviceUser);
                    };
                }
                if (context.Roles != null && !context.Roles.Any())
                {
                    adminRole = new IdentityRole()
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    };
                    serviceRole = new IdentityRole()
                    {
                        Name = "Service",
                        NormalizedName = "SERVICE"
                    };
                    var roles = new List<IdentityRole>()
                    {
                        adminRole,
                        serviceRole,
                        new IdentityRole()
                        {
                            Name = "Buyer",
                            NormalizedName = "BUYER"
                        }
                    };
                    context.AddRange(roles);
                    //context.Add(adminRole);
                }
                if (adminUser != null && adminRole != null)
                {
                    context.UserRoles.Add(new IdentityUserRole<string>()
                    {
                        RoleId = adminRole.Id,
                        UserId = adminUser.Id
                    });
                }
                if (serviceUser != null && serviceRole != null)
                {
                    context.UserRoles.Add(new IdentityUserRole<string>()
                    {
                        RoleId = serviceRole.Id,
                        UserId = serviceUser.Id
                    });
                }
                context.SaveChanges();
            }
        }
    }
}

