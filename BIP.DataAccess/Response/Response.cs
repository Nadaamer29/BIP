namespace BIP.Response
{
    public class Response<T>
    {
        public T Data { get; set; }
        public object Meta { get; set; }
        public string Message { get; set; }
        public bool Succeeded { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }
    }

}
