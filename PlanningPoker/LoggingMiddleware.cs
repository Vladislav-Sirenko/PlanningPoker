using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PlanningPoker
{
    internal class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                [nameof(context.TraceIdentifier)] = context.TraceIdentifier,
                [nameof(context.Request.Path)] = context.Request.Path,
            }))
            {
                try
                {
                    _logger.LogInformation("Request received.");
                    if (context.Request.QueryString.HasValue)
                    {
                        _logger.LogInformation($"Query string: '{context.Request.QueryString}'");
                    }

                    await _next(context);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unhandled exception: ");
                    throw;
                }
            }
        }
    }
}
