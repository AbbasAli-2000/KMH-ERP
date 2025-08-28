using Dapper;
using KMH_ERP.Application.DTOs;
using KMH_ERP.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMH_ERP.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUserAsync();
        Task<UserDto?> FindUserAsync(int id);
        Task<int> SaveUserAsync(UserDto userDto);
        Task<bool> GetByEmailAsync(string email, int? userId = null);
        Task<bool> GetByPhoneNumberAsync(string phoneNumber, int? userId = null);
    }
}
