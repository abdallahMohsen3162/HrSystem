using BusinessLayer.Interfaces;
using DataLayer.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Threading.Tasks;

namespace BusinessLayer.Seeding
{
    public class SuperAdminSeeder
    {
        private readonly IRolesService rolesService;
        private readonly IAccountsService accountsService;

        public SuperAdminSeeder(IRolesService rolesService, IAccountsService accountsService)
        {
            this.rolesService = rolesService;
            this.accountsService = accountsService;
        }

        public async Task SeedAsync()
        {
            try
            {

                var role = await rolesService.GetRoleByNameAsync("SuperAdmin");
                if (role == null)
                {
                    await rolesService.CreateRoleAsync("SuperAdmin");
                    Console.WriteLine("SuperAdmin role created.");
                }
                else
                {
                    Console.WriteLine("SuperAdmin role already exists.");
                }


                var roles = rolesService.GetRoles();
                foreach (var r in roles)
                {
                    Console.WriteLine($"{r.Id}: {r.Name}");
                }

                 Console.WriteLine("Creating SuperAdmin user...");
                var model = new CreateUserViewModel
                {
                    Username = "Admin@yopmail.com",
                    FullName = "Super Administrator",
                    Email = "Admin@yopmail.com",
                    Password = "123456",  
                    ConfirmPassword = "123456",
                    Role = "SuperAdmin"
                };

                await accountsService.CreateUserAsync(model);
                Console.WriteLine("SuperAdmin user created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding SuperAdmin: {ex.Message}");
            }
        }
    }
}
