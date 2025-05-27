using AdminBack.Models;
using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface IRoleService
    {
        Task<ApiResponse<List<Role>>> GetAllRolesAsync();
        Task<ApiResponse<Role>> GetRoleByIdAsync(int id);
        Task<ApiResponse<Role>> CreateRoleAsync(Role role);
        Task<ApiResponse<Role>> UpdateRoleAsync(int id, Role roleUpdate);
        Task<ApiResponse<bool>> DeleteRoleAsync(int id);
        Task<ApiResponse<List<Usuario>>> GetUsersByRoleAsync(int roleId);
    }
}