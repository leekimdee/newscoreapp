using Dapper;
using Microsoft.Extensions.Configuration;
using NewsCoreApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Application
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IConfiguration _configuration;

        public UserRoleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task RemoveRolesOfUserAsync(string userId, string roles)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@userId", userId);
                dynamicParameters.Add("@roles", roles);

                try
                {
                    await sqlConnection.QueryAsync("RemoveRolesOfUser", dynamicParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
