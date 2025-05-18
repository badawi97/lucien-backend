namespace Lucien.Application.Contracts.Common.Api
{
    public class Response<T> where T : class
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
    }
}
