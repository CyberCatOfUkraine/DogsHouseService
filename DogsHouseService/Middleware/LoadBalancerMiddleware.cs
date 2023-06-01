using System.Net;

namespace DogsHouseService.Middleware
{
    public class LoadBalancerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxRequestsPerSecond;
        private static int _requestCount;
        private Timer _timer;
        public LoadBalancerMiddleware(RequestDelegate next, int maxRequestsPerSecond)
        {
            _next = next;
            _maxRequestsPerSecond = maxRequestsPerSecond;
            _timer = new Timer(ResetRequestCount, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }


        public async Task Invoke(HttpContext context)
        {
            UpdateRequestCount();
            if (_requestCount < _maxRequestsPerSecond)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("Too Many Requests");
                await Console.Out.WriteLineAsync("Too Many Requests");
            }
        }

        private void ResetRequestCount(object? state)
        {
            _requestCount = 0;
        }
        private async void UpdateRequestCount()
        {
            int count = Interlocked.Add(ref _requestCount, 1);
            await Console.Out.WriteLineAsync($"Requests per second: {count}");
        }
    }
}