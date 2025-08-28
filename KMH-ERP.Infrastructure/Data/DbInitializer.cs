using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Domain.Entities;
using KMH_ERP.Infrastructure.Utilities.Halpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMH_ERP.Infrastructure.Data
{
    public static class DbInitializer
    {

        public static async Task SeedAsync(IUnitOfWork unitOfWork)
        {

            var role = await unitOfWork.EFRepo<Role>().GetAllAsync();
            if (!role.Any())
            {

                await unitOfWork.EFRepo<Role>().AddOrUpdateAsync(new Role
                {
                    RoleName = "SuperAdmin",
                    Description = "Super Administrator",
                    IsActive = true,
                    CreatedBy = 0,
                    CreatedAt = DateTime.Now
                });
                await unitOfWork.SaveChangesAsync();

            }

            var user = await unitOfWork.EFRepo<User>().GetAllAsync();
            if (!user.Any())
            {
                var superAdminRole = (await unitOfWork.EFRepo<Role>().GetAllAsync()).FirstOrDefault(r => r.RoleName == "SuperAdmin");
                if (superAdminRole != null)
                {

                    await unitOfWork.EFRepo<User>().AddOrUpdateAsync(new User
                    {
                        UserName = "superadmin",
                        PasswordHash = PasswordHelper.HashPassword("Admin123+"),
                        Email = "superadmin@kmh.com",
                        RoleId = superAdminRole.RoleId,
                        IsActive = true,
                        CreatedBy = 0,
                        CreatedAt = DateTime.Now,
                    });

                    await unitOfWork.SaveChangesAsync();

                }
            }
        }
    }
}
