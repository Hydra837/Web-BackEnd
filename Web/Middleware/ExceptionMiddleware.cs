namespace Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Set the response status code
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // Define a standard response format
                var response = new ErrorResponse(
                    statusCode: StatusCodes.Status500InternalServerError,
                    message: "An internal server error occurred.",
                    details: ex.Message // Optional: include exception details if needed
                );

                // Write the JSON response
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
