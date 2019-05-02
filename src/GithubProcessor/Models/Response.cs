namespace GithubProcessor.Models
{
    public class Response
    {
        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public static Response GetSuccessfulResponse(string message = null)
        {
            return new Response()
            {
                IsSuccessful = true,
                Message = message,
            };
        }

        public static Response GetUnSuccessfulResponse(string message = null)
        {
            return new Response()
            {
                IsSuccessful = false,
                Message = message,
            };
        }
    }
}
