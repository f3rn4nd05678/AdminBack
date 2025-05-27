using AdminBack.Models.DTOs;

namespace AdminBack.Utils
{
    public static class ResponseHelper
    {
        public static ApiResponse<T> Success<T>(T detail, string message = "Operación exitosa", int statusCode = 200)
        {
            return new ApiResponse<T>(true, message, statusCode, detail);
        }

        public static ApiResponse<T> Fail<T>(string message = "Ocurrió un error", int statusCode = 400)
        {
            return new ApiResponse<T>(false, message, statusCode, default);
        }
    }
}
