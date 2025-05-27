namespace AdminBack.Models.DTOs
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Detail { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Operación exitosa", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Fail(string message, List<string>? detail = null, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Message = message,
                Detail = detail
            };
        }
    }
}
