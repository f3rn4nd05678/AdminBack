using AdminBack.Models;
using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface IUserService
    {
        Task<ApiResponse<List<Usuario>>> GetAllUsersAsync();
        Task<ApiResponse<Usuario>> GetUserByIdAsync(int id);
        Task<ApiResponse<Usuario>> UpdateUserAsync(int id, Usuario usuarioUpdate);
        Task<ApiResponse<bool>> ToggleUserStatusAsync(int id);
        Task<ApiResponse<bool>> DeleteUserAsync(int id);
        Task<ApiResponse<bool>> ResetPasswordAsync(int id, string nuevaContrasena);
        Task<ApiResponse<List<Usuario>>> SearchUsersAsync(string searchTerm);
    }
}