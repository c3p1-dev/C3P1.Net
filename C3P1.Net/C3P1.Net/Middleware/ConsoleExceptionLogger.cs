namespace C3P1.Net.Middleware
{
    // Usage :
    // app.UseMiddleware<ConsoleExceptionLogger>();
    // in Program.cs, before classic Middlewares
    public class ConsoleExceptionLogger(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UriFormatException ex)
            {
                LogRequest(context, ex, "URI ERROR");
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Bad Request: Invalid URI");
            }
            catch (Exception ex)
            {
                LogRequest(context, ex, "UNHANDLED ERROR");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Erreur interne du serveur.");
            }
        }

        private static void LogRequest(HttpContext context, Exception ex, string title)
        {
            Console.WriteLine("======================================");
            Console.WriteLine($"[{title}] {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($" - Path     : {context.Request.Path}");
            Console.WriteLine($" - Method   : {context.Request.Method}");
            Console.WriteLine($" - Host     : {context.Request.Host}");
            Console.WriteLine($" - Query    : {context.Request.QueryString}");

            Console.WriteLine(" - Headers  :");
            foreach (var header in context.Request.Headers)
            {
                Console.WriteLine($"    {header.Key}: {header.Value}");
            }

            if (title == "URI ERROR")
            {
                Console.WriteLine($" - Message  : {ex.Message}");
            }
            else
            {
                Console.WriteLine($" - Exception: {ex}");
            }

            Console.WriteLine("======================================");
        }
    }

}
