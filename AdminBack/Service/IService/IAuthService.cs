using AdminBack.Models;

namespace AdminBack.Service.IService
{
    public interface IAuthService
    {
        string GenerateToken(Usuario usuario);
    }
}
