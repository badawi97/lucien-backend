namespace Lucien.Application.Contracts.Common.Dto
{
    public class ResultDto<T>
    {
        /// <summary>
        /// The payload or result data. Can be null if the operation failed or there's no content.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// A message describing the result, success or failure.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Optional HTTP-like status code for extra context (logging, internal use).
        /// </summary>
        public int? StatusCode { get; set; }

        // -------------------------------
        // Factory Methods
        // -------------------------------

        public static ResultDto<T> Success(T data, string message = "Success", int? statusCode = 200)
        {
            return new ResultDto<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ResultDto<T> Failure(string message = "An error occurred", int? statusCode = 400)
        {
            return new ResultDto<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default,
                StatusCode = statusCode
            };
        }

        public static ResultDto<T> Empty(string message = "No content", int? statusCode = 204)
        {
            return new ResultDto<T>
            {
                IsSuccess = true,
                Message = message,
                Data = default,
                StatusCode = statusCode
            };
        }
    }
}
