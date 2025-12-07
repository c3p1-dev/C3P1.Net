namespace C3P1.Net.Middleware
{
    public static class PrecompressedStaticFilesMiddlewareExtensions
    {
        public static IApplicationBuilder UsePrecompressedStaticFiles(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PrecompressedStaticFilesMiddleware>();
        }
    }

    public class PrecompressedStaticFilesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _wwwrootPath;

        public PrecompressedStaticFilesMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _wwwrootPath = env.WebRootPath ?? "wwwroot";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Ne traiter que GET et HEAD
            if (!HttpMethods.IsGet(context.Request.Method) && !HttpMethods.IsHead(context.Request.Method))
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value;
            if (string.IsNullOrEmpty(path))
            {
                await _next(context);
                return;
            }

            // Ne gérer que les fichiers ciblés
            if (!path.EndsWith(".wasm") && !path.EndsWith(".dll") &&
                !path.EndsWith(".pdb") && !path.EndsWith(".js") &&
                !path.EndsWith(".css"))
            {
                await _next(context);
                return;
            }

            var acceptEncoding = context.Request.Headers["Accept-Encoding"].ToString();

            string? filePath = null;
            string? contentEncoding = null;

            // Priorité Brotli
            if (acceptEncoding.Contains("br"))
            {
                var brFile = Path.Combine(_wwwrootPath, path.TrimStart('/')) + ".br";
                if (File.Exists(brFile))
                {
                    filePath = brFile;
                    contentEncoding = "br";
                }
            }

            // Sinon gzip
            if (filePath == null && acceptEncoding.Contains("gzip"))
            {
                var gzFile = Path.Combine(_wwwrootPath, path.TrimStart('/')) + ".gz";
                if (File.Exists(gzFile))
                {
                    filePath = gzFile;
                    contentEncoding = "gzip";
                }
            }

            // Sinon fallback raw
            if (filePath == null)
            {
                filePath = Path.Combine(_wwwrootPath, path.TrimStart('/'));
                if (!File.Exists(filePath))
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.Headers["Cache-Control"] = "public,max-age=31536000";
            if (contentEncoding != null)
                context.Response.Headers["Content-Encoding"] = contentEncoding;

            var contentType = GetContentType(filePath);
            if (contentType != null)
                context.Response.ContentType = contentType;

            await context.Response.SendFileAsync(filePath);
        }

        private static string? GetContentType(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            if (ext == ".br" || ext == ".gz")
                ext = Path.GetExtension(Path.GetFileNameWithoutExtension(filePath));

            return ext switch
            {
                ".wasm" => "application/wasm",
                ".dll" => "application/octet-stream",
                ".pdb" => "application/octet-stream",
                ".js" => "application/javascript",
                ".css" => "text/css",
                _ => null
            };
        }
    }
}
