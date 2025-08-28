using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Application.Interfaces.Services;
using KMH_ERP.Application.Services;
using KMH_ERP.Infrastructure.Data;
using KMH_ERP.Infrastructure.Persistence;
using KMH_ERP.Infrastructure.Repositories.DapperRepository;
using KMH_ERP.Infrastructure.Repositories.EFRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMH_ERP.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            //Database Connection (EF Core)
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("KMH_ERPDbConnDefault")));


            // Dapper Connection
            //services.AddSingleton<Dapper_DbConnection>(sp =>
            //    new Dapper_DbConnection(sp.GetRequiredService<IConfiguration>(), "KMH_ERPDbConnDefault"));

            services.AddSingleton<Dapper_DbConnection>();

            services.AddScoped<IDbConnection>(sp =>
            {
                var conn = sp.GetRequiredService<Dapper_DbConnection>().GetConnection();
                conn.Open();
                return conn;
            });

            //Repositories
            services.AddScoped(typeof(IEFGenericRepository<>), typeof(EFGenericRepository<>));
            services.AddScoped(typeof(IDapperRepository<>), typeof(DapperRepository<>));


            //Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Services 
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
