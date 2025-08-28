using Microsoft.Extensions.Configuration;
using KMH_ERP.Application.DTOs;
using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Application.Interfaces.Services;
using KMH_ERP.Domain.Entities;
using KMH_ERP.Infrastructure.Utilities.Halpers;

namespace KMH_ERP.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<(UserDto, string token)> LoginAsync(UserLoginDto userLoginDto)
        {
            try
            {
                var user = await (_unitOfWork.EFRepo<User>().GetAsync(u => u.Email == userLoginDto.Email && u.IsActive == true));
                if (user == null || !PasswordHelper.VerifyPassword(userLoginDto.Password, user.PasswordHash))
                {
                    await AddLogActivity(new LoginActivityDto
                    {
                        UserId = user?.UserId,
                        Username = user?.UserName,
                        LoginTime = DateTime.Now,
                        IPAddress = userLoginDto.ipAddress,
                        IsSuccess = false,
                        FailureReason = "Invalid credentials"
                    });
                    return (null, null);
                }

                var role = await _unitOfWork.EFRepo<Role>().GetAsync(u => u.RoleId == user.RoleId && u.IsActive == true);

                user.LastLogin = DateTime.Now;
                _unitOfWork.EFRepo<User>().Update(user);
                await _unitOfWork.SaveChangesAsync();


                var token = JwtHelper.GenerateJwtToken(user.UserName, role.RoleName);
                await SaveJwtToken(new JwtLoginToken
                {
                    UserId = user.UserId,
                    Token = token
                });

                await AddLogActivity(new LoginActivityDto
                {
                    UserId = user.UserId,
                    Username = user.UserName,
                    LoginTime = DateTime.Now,
                    IsSuccess = true,
                    FailureReason = null
                });

                var userData = new UserDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    RoleName = role.RoleName,
                    IsActive = user.IsActive,
                    IsRemote = user.IsRemote,
                    IsLocked = user.IsRemote
                };
                return (userData, token);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while adding log activity.", ex);
            }
        }

        public async Task LogoutAsync(int userId, string? ipAddress = null)
        {
            var activeToken = await _unitOfWork.EFRepo<JwtLoginToken>().GetAsync(t => t.UserId == userId && t.IsActive);
            if (activeToken != null)
            {
                activeToken.IsActive = false;
                activeToken.ExpiryAt = DateTime.Now;
                _unitOfWork.EFRepo<JwtLoginToken>().Update(activeToken);
                await _unitOfWork.SaveChangesAsync();
            }

            var user = await (_unitOfWork.EFRepo<User>().GetAsync(u => u.UserId == userId && u.IsActive == true));
            // Activity Log (Logout)
            await AddLogActivity(new LoginActivityDto
            {
                UserId = userId,
                LogOutTime = DateTime.Now,
                IPAddress = ipAddress,
                IsSuccess = true,
                FailureReason = null
            });
        }


        #region Add Log
        public async Task<bool> AddLogActivity(LoginActivityDto loginActivityDto)
        {
            try
            {
                var repo = _unitOfWork.EFRepo<LoginActivity>();

                var lastLogin = await _unitOfWork.EFRepo<LoginActivity>().FindAsync(l => l.UserId == loginActivityDto.UserId && l.LogoutTime == null,
                                                                                      q => q.OrderByDescending(x => x.LoginTime));

                if (lastLogin != null && loginActivityDto.LogOutTime != null)
                {
                    // Update logout time of last login record
                    lastLogin.LogoutTime = loginActivityDto.LogOutTime;
                    lastLogin.IsSuccess = loginActivityDto.IsSuccess;
                    lastLogin.FailureReason = loginActivityDto.FailureReason;
                    lastLogin.IPAddress = loginActivityDto.IPAddress;
                    repo.Update(lastLogin);
                }
                else
                {
                    // Insert new login activity
                    var entity = new LoginActivity
                    {
                        UserId = loginActivityDto.UserId,
                        Username = loginActivityDto.Username,
                        LoginTime = loginActivityDto.LoginTime,
                        LogoutTime = loginActivityDto.LogOutTime,
                        IPAddress = loginActivityDto.IPAddress,
                        IsSuccess = loginActivityDto.IsSuccess,
                        FailureReason = loginActivityDto.FailureReason
                    };
                    await repo.AddAsync(entity);
                }
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while adding log activity.", ex);
            }
        }
        #endregion


        #region Save JWT Token
        public async Task SaveJwtToken(JwtLoginToken jwtToken)
        {
            try
            {
                // Check if user already has an active token
                var existingToken = await _unitOfWork.EFRepo<JwtLoginToken>().GetAsync(t => t.UserId == jwtToken.UserId && t.IsActive);
                if (existingToken != null)
                {
                    existingToken.Token = jwtToken.Token;
                    existingToken.IssuedAt = DateTime.Now;
                    existingToken.ExpiryAt = DateTime.Now.AddMinutes(60);
                    existingToken.IsActive = true;

                    _unitOfWork.EFRepo<JwtLoginToken>().Update(existingToken);
                }
                else
                {
                    jwtToken.IsActive = true;
                    jwtToken.IssuedAt = DateTime.Now;
                    jwtToken.ExpiryAt = DateTime.Now.AddMinutes(60);
                    await _unitOfWork.EFRepo<JwtLoginToken>().AddAsync(jwtToken);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while saving JWT token.", ex);
            }
        }

        #endregion
    }
}
