using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using MinimalChatApp.Business;
using MinimalChatApp.Model;
using MinimalChatApplication.Model;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Cors;

namespace MinimalChatApp.Business
{
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class RequestLoggingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context, ChatDBContext dbcontext)
        {
            try
            {


                //await context.Response.WriteAsJsonAsync<Logs>(logs);
                // Log request details
             
                LogRequest(context, dbcontext);
                using var buffer = new MemoryStream();
                var response = context.Response;
             
                var stream = response.Body;
                response.Body = buffer;
                await _next(context);
                buffer.Position = 0;
                await buffer.CopyToAsync(stream);
             
            }
            catch (Exception ex)
            {
               // context.Response.StatusCode = 500;
                var errorInfo = new ErrorInfo()
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = ex.Message
                };

                var errorLog = new ErrorLogger()
                {
                    ErrorDetails = errorInfo.ErrorMessage,
                    LogDate = DateTime.Now
                };
                await dbcontext.ErrorLogs.AddAsync(errorLog);
                await dbcontext.SaveChangesAsync();
                using var buffer = new MemoryStream();
                var response = context.Response;

                var stream = response.Body;
                response.Body = buffer;
                await _next(context);
                buffer.Position = 0;
                await buffer.CopyToAsync(stream);

            }
        }
        private void LogRequest(HttpContext context, ChatDBContext dbcontext)
        {
            // Get IP of caller
            string username = _httpContextAccessor.HttpContext.User.Identity.Name;
            // Capture request details
            var request = context.Request;
            var method = request.Method;
            var path = request.Path;
            var queryString = request.QueryString;
            //using var buffer1 = new MemoryStream();
            var ipAddress = context.Connection.RemoteIpAddress.ToString();

            // Get request body
            string requestBody = ReadRequestBody(context.Request);


            Logs logs = new Logs()
            {
                Ipaddress = ipAddress,
                RequestBody = "Method:- " + method + " Path:-" + path + " Request:- " + requestBody,
                CreatedDate = DateTime.Now,
                CreatedBy = username
            };
            // Read request body
            dbcontext.Logs.AddAsync(logs);
            dbcontext.SaveChangesAsync();

        }

        private string ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering(); // Enable rewinding the request body stream
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                string requestBody = reader.ReadToEnd();
                request.Body.Seek(0, SeekOrigin.Begin); // Reset the request body stream position
                return requestBody;
            }
        }

    }




    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
