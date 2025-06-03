
using System.Diagnostics;

namespace Restaurants.API.MiddleWares
{
    public class TimeLoggingMiddle(ILogger<TimeLoggingMiddle> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var sec =  Stopwatch.StartNew();
            await next.Invoke(context); 
            sec.Stop();
             
            if(sec.ElapsedMilliseconds > 4000)
            {
                logger.LogInformation("Request [{verb}] at {path} took {Time} ms", 
                    context.Request.Method,
                    context.Request.Path,
                    sec.ElapsedMilliseconds
                    );
            }
        }
    }
}
