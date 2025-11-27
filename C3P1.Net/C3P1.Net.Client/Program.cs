using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazr.RenderState.WASM;
using C3P1.Net.Client.Services.Layout;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace C3P1.Net.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddAuthenticationStateDeserialization();
            
            // Add Blazorise related services
            builder.Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                    // unsafe way to store product token
                    // TODO : figure out how to store secret strings in wasm
                    options.ProductToken = builder.Configuration["Blazorise:ProductToken"] ?? throw new InvalidOperationException("Blazorise license token 'ProductToken' not found.");                    
                })
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();

            // Add BlazR renderstate service
            builder.AddBlazrRenderStateWASMServices();

            // Add NavBreadcrumb service
            builder.Services.AddScoped<NavBreadcrumbService>();

            // Add HttpClient service
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            // Run the app
            await builder.Build().RunAsync();
        }
    }
}
