using KMH_ERP.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMH_ERP.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<int> SaveRoleAsync(RoleDto  roleDto);
        Task<List<RoleDto>> GetRolesAsync();
        Task<RoleDto?> FindRoleAsync(int id);
    }
}
