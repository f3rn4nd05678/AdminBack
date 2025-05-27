using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AdminBack.Service
{
    public class AuthService : IAuthService
    {
        private readonly AdminDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AdminDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Activo == true);

                if (usuario == null)
                {
                    return ApiResponse<LoginResponseDto>.Fail("Credenciales inválidas", null, 401);
                }

                if (!VerifyPassword(loginDto.Password, usuario.Contrasena))
                {
                    return ApiResponse<LoginResponseDto>.Fail("Credenciales inválidas", null, 401);
                }

                var token = GenerateJwtToken(usuario);

                var response = new LoginResponseDto
                {
                    Token = token,
                    Usuario = usuario
                };

                return ApiResponse<LoginResponseDto>.Success(response, "Login exitoso");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<Usuario>> RegisterAsync(UsuarioRegisterDto registerDto)
        {
            try
            {
                var existeEmail = await _context.Usuarios.AnyAsync(u => u.Email == registerDto.Email);
                if (existeEmail)
                {
                    return ApiResponse<Usuario>.Fail("El correo electrónico ya está registrado");
                }

                // Verificar que el rol existe si se proporciona
                if (registerDto.RolId.HasValue)
                {
                    var rolExiste = await _context.Roles.AnyAsync(r => r.Id == registerDto.RolId.Value);
                    if (!rolExiste)
                    {
                        return ApiResponse<Usuario>.Fail("El rol especificado no existe");
                    }
                }

                var hashedPassword = HashPassword(registerDto.Contrasena);

                var nuevoUsuario = new Usuario
                {
                    NombreCompleto = registerDto.NombreCompleto,
                    Email = registerDto.Email,
                    Contrasena = hashedPassword,
                    RolId = registerDto.RolId,
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                // Cargar el rol para la respuesta
                await _context.Entry(nuevoUsuario)
                    .Reference(u => u.Rol)
                    .LoadAsync();

                return ApiResponse<Usuario>.Success(nuevoUsuario, "Usuario registrado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<Usuario>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(int usuarioId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                {
                    return ApiResponse<bool>.Fail("Usuario no encontrado", null, 404);
                }

                if (!VerifyPassword(changePasswordDto.PasswordActual, usuario.Contrasena))
                {
                    return ApiResponse<bool>.Fail("La contraseña actual es incorrecta", null, 400);
                }

                usuario.Contrasena = HashPassword(changePasswordDto.PasswordNueva);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Success(true, "Contraseña cambiada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            if (usuario.Rol != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, usuario.Rol.Nombre));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hashedPassword);
        }
    }
}