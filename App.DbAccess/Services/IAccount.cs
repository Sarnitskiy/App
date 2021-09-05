using App.DbAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.DbAccess.Services
{
    public interface IAccount
    {
        Task<AccountUser> GetUserAsync(string email, string password);
        Task<IEnumerable<AccountRole>> GetUserRolesAsync(int userId);
        Task AddUserRoleAsync(int userId, int roleId);
        Task DeleteUserRoleAsync(int userId, int roleId);
    }
}
