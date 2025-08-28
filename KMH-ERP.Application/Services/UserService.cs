using Dapper;
using KMH_ERP.Application.DTOs;
using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Application.Interfaces.Services;
using KMH_ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMH_ERP.Application.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public async Task<List<UserDto>> GetUserAsync()
        {
            try
            {

                var result = await _unitOfWork.DapperRepo<UserDto>().GetAllQueryAsync("SP_GET_User");
                return result.ToList();

            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error occurred while adding Role.", ex);
            }
        }

        public async Task<UserDto?> FindUserAsync(int id)
        {
            try
            {
                var parameters = new { UserId = id };
                var result = await _unitOfWork.DapperRepo<UserDto>().GetSingleAsync("SP_GET_User", parameters);
                return result;

            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error occurred while adding Role.", ex);
            }
        }

        public async Task<bool> GetByEmailAsync(string email, int? userId = null)
        {
            try
            {
                var result = await _unitOfWork.EFRepo<User>()
                    .GetAsync(u => u.Email.ToLower() == email.ToLower()
                                && (userId == null || u.UserId != userId));

                return result != null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while checking email.", ex);
            }
        }

        public async Task<bool> GetByPhoneNumberAsync(string phoneNumber, int? userId = null)
        {
            try
            {
                var result = await _unitOfWork.EFRepo<User>()
                    .GetAsync(u => u.PhoneNumber == phoneNumber
                                && (userId == null || u.UserId != userId));

                return result != null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while checking phone number.", ex);
            }
        }

        public async Task<int> SaveUserAsync(UserDto userDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userDto.UserId);
                parameters.Add("@EmployeeId", userDto.EmployeeId);
                parameters.Add("@UserName", userDto.UserName);
                parameters.Add("@PasswordHash", userDto.PasswordHash);
                parameters.Add("@Email", userDto.Email);
                parameters.Add("@PhoneNumber", userDto.PhoneNumber);
                parameters.Add("@MobileNumber", userDto.MobileNumber);
                parameters.Add("@RoleId", userDto.RoleId);
                parameters.Add("@IsActive", userDto.IsActive);
                parameters.Add("@IsLocked", userDto.IsLocked);
                parameters.Add("@IsRemote", userDto.IsRemote);
                parameters.Add("@LockDate", userDto.LockDate);
                parameters.Add("@FailedLoginAttempts", userDto.FailedLoginAttempts);
                parameters.Add("@Action", userDto.UserId == 0 ? "INSERT" : "UPDATE");
                parameters.Add("@CreatedBy", userDto.UserId);
                parameters.Add("@ModifiedBy", userDto.UserId);
                var outParam = "@OutUserId";
                var result = await _unitOfWork.DapperRepo<UserDto>().CreateAsync("SP_DML_User", parameters, outParam);
                return result;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error occurred while adding Role.", ex);
            }
        }
    }
}
