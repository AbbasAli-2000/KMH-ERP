using Dapper;
using KMH_ERP.Application.DTOs;
using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Application.Interfaces.Services;

namespace KMH_ERP.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleDto?> FindRoleAsync(int id)
        {
            try
            {
                var parameters = new { RoleId = id };
                var result = await _unitOfWork.DapperRepo<RoleDto>().GetSingleAsync("SP_Find_Role", parameters);
                return result;

            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error occurred while adding Role.", ex);
            }
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            try
            {

                var result = await _unitOfWork.DapperRepo<RoleDto>().GetAllQueryAsync("SP_Find_Role");
                return result.ToList();

            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error occurred while adding Role.", ex);
            }
        }

        public async Task<int> SaveRoleAsync(RoleDto roleDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", roleDto.RoleId);
                parameters.Add("@RoleName", roleDto.RoleName);
                parameters.Add("@Description", roleDto.Description);
                parameters.Add("@IsActive", roleDto.IsActive);
                parameters.Add("@Action", roleDto.RoleId == 0 ? "INSERT" : "UPDATE");
                parameters.Add("@CreatedBy", roleDto.UserId);
                parameters.Add("@ModifiedBy", roleDto.UserId);
                var outParam = "@OutRoleId";
                var result = await _unitOfWork.DapperRepo<RoleDto>().CreateAsync("SP_DML_Role", parameters, outParam);
                return result;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error occurred while adding Role.", ex);
            }
        }
    }
}
