using UrlShortener.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data.Config
{

    public static class AuthConfig
    {
        //Admin class will be an admin account automatically created if one is not already created in the database
        internal static class Admin
        {
            public const string userName = "admin";
            public const string password = "TenDigitPassword";
            public const string roleName = "Administrator";

            public const string email = "admin@admin.com";

            public static SiteUser SiteUserObject() => new SiteUser {UserName = userName, FirstName = "root", LastName = "default-admin-account", Email = email};
        }

        //ConfigAdmin takes the Admin account created about and adds it to the database
        public static async Task ConfigAdmin(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetService<UserManager<SiteUser>>();

            if (await roleManager.FindByNameAsync(Admin.roleName) == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(Admin.roleName));
                if (result.Succeeded)
                {
                    Console.WriteLine($"{Admin.userName} account Added to DB");
                }
                else
                {
                    Console.WriteLine($"{Admin.userName} is already created in DB");
                }
            }

            if (await userManager.FindByNameAsync(Admin.userName) == null)
            {
                SiteUser adminUser = Admin.SiteUserObject();
                var result = await userManager.CreateAsync(adminUser, Admin.password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Admin.roleName);
                    Console.WriteLine($"{Admin.roleName} added to DB");
                }
                else
                {
                    Console.WriteLine("User & role not added. Database or program error.");
                }
            }
        }

    }

}