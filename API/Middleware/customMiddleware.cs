using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class customMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<customMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
      public customMiddleware(RequestDelegate next,
       ILogger<customMiddleware> logger, IWebHostEnvironment env)
      {
            _env = env;
            _logger = logger;
            _next = next;
      }  

      public async Task InvokeAsync(HttpContext context)
      {
        try{
         await _next(context);
        }//sits at top of stacktrace tree and track the error oject
        catch(Exception ex)
        {
        _logger.LogError(ex, ex.Message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var response = new ProblemDetails {
            Status = 500,
            Detail = _env.IsDevelopment() ? ex.StackTrace.ToString() : null,
            Title = ex.Message,
        };

        var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        var jsonResult = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(jsonResult);
        }
      }
    }
}