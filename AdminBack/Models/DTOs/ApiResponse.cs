namespace AdminBack.Models.DTOs
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T? Detail { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(bool isSuccess, string message, int statusCode, T? detail = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
            Detail = detail;

        }
    }
}
