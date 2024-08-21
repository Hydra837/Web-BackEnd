namespace Web.Middleware
{
    internal class ErrorResponse
    {
        private int statusCode;
        private string message;
        private string details;

        public ErrorResponse(int statusCode, string message, string details)
        {
            this.statusCode = statusCode;
            this.message = message;
            this.details = details;
        }
    }
}