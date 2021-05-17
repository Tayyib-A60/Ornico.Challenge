namespace Models.ViewModels
{
    public class BaseResponse<T> where T : class
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
