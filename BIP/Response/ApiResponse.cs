namespace BIP.Response
{
    public class ApiResponse
    {
        public object Data { get; set; }
        public object Pagination { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public object Meta { get; set; }
    }
}
