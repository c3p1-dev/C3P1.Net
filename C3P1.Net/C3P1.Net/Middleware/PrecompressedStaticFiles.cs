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
            //Console.WriteLine($"[PrecompressedStaticFiles] Request Path: {context.Request.Path}"); ;
            // On ne gère que les GET et HEAD
            if (!HttpMethods.IsGet(context.Request.Method) && !HttpMethods.IsHead(context.Request.Method))
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value;

            // targeted files for blazor (wasm, dll, pdb, js, css)
            if (path != null &&
                (path.EndsWith(".wasm") || path.EndsWith(".dll") || path.EndsWith(".pdb") || path.EndsWith(".js") || path.EndsWith(".css")))
            {
                var acceptEncoding = context.Request.Headers["Accept-Encoding"].ToString();

                string? filePath = null;
                string? contentEncoding = null;

                // brotli priority
                if (acceptEncoding.Contains("br"))
                {
                    var brFile = Path.Combine(_wwwrootPath, path.TrimStart('/')) + ".br";
                    if (File.Exists(brFile))
                    {
                        filePath = brFile;
                        contentEncoding = "br";
                    }
                }

                // or gzip
                if (filePath == null && acceptEncoding.Contains("gzip"))
                {
                    var gzFile = Path.Combine(_wwwrootPath, path.TrimStart('/')) + ".gz";
                    if (File.Exists(gzFile))
                    {
                        filePath = gzFile;
                        contentEncoding = "gzip";
                    }
                }

                // or fallback on raw file
                if (filePath == null)
                {
                    filePath = Path.Combine(_wwwrootPath, path.TrimStart('/'));
                    if (!File.Exists(filePath))
                    {
                        await _next(context);
                        return;
                    }
                }

                //Console.WriteLine($"[PrecompressedStaticFiles] Serving: {filePath} (Content-Encoding: {contentEncoding ?? "none"})");

                context.Response.Headers["Cache-Control"] = "public,max-age=31536000";
                if (contentEncoding != null)
                    context.Response.Headers["Content-Encoding"] = contentEncoding;

                var contentType = GetContentType(filePath);
                if (contentType != null)
                    context.Response.ContentType = contentType;

                await context.Response.SendFileAsync(filePath);
                return;
            }

            // go to next middleware
            await _next(context);
        }

        private static string? GetContentType(string filePath)
        {
            var ext = Path.GetExtension(filePath); // ext = ".br" ou ".gz"
            if (ext == ".br" || ext == ".gz")
                ext = Path.GetExtension(Path.GetFileNameWithoutExtension(filePath)); // get real extension

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