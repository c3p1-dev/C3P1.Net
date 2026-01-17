namespace C3P1.Net.Middleware
{
    // Usage :
    // app.UseMiddleware<DenyEmptyHost>();
    // in Program.cs, before classic Middlewares
    public class DenyEmptyHost(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            // deny connections with missing header Host before trying to manage exceptions
            if (string.IsNullOrEmpty(context.Request.Host.Host))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Bad Request : Missing Host header");
                return;
            }

            await next(context);
        }
    }
}