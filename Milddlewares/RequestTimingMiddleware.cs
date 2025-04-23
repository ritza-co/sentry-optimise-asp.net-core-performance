using System.Diagnostics;

namespace TodoApi.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();
            if (sw.ElapsedMilliseconds > 500)
            {
                _logger.LogWarning("⚠️ Slow request: {Path} took {Elapsed}ms", context.Request.Path, sw.ElapsedMilliseconds);
            }
        }
    }
}