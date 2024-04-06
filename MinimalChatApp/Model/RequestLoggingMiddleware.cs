using System.Text;

namespace MinimalChatApp.Model
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            
            var requestBody = await FormatRequest(context.Request);
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var requestTime = DateTime.Now;
            var username = context.User.Identity.Name ?? "Anonymous";

            _logger.LogInformation($"IP: {ipAddress}, Request Body: {requestBody}, Time: {requestTime}, Username: {username}");

            await _next(context);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return body;
            }
        }

        

    }
}
