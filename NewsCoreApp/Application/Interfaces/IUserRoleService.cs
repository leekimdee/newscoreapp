using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Application.Interfaces
{
    public interface IUserRoleService
    {
        Task RemoveRolesOfUserAsync(string userId, string roles);
    }
}
