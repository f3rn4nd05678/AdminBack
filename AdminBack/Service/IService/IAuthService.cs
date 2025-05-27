using AdminBack.Models;
using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<Usuario>> RegisterAsync(UsuarioRegisterDto registerDto);
        Task<ApiResponse<bool>> ChangePasswordAsync(int usuarioId, ChangePasswordDto changePasswordDto);
    }
}