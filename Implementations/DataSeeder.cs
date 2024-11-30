using ApiLogin.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ApiLogin.Implementations
{
    public class DataSeeder : IDataSeeder
    {
        public async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var rolesToCreate = new[] { "Administrator" };

            foreach (var roleName in rolesToCreate)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

    }
}