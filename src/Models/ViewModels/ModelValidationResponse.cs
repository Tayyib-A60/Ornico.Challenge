namespace Models.ViewModels
{
    public class ModelValidationResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }
}
