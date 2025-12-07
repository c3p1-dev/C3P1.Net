using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazr.RenderState.Server;
using C3P1.Net.Client.Services.Layout;
using C3P1.Net.Components;
using C3P1.Net.Components.Account;
using C3P1.Net.Data;
using C3P1.Net.Middleware;
using C3P1.Net.Services.Admin;
using C3P1.Net.Services.Apps;
using C3P1.Net.Services.Apps.BankBook;
using C3P1.Net.Shared.Services.Admin;
using C3P1.Net.Shared.Services.Apps;
using C3P1.Net.Shared.Services.Apps.BankBook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace C3P1.Net
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Init SQLitePCLRaw_bundle_sqlite (= use system SQLite)
            SQLitePCL.Batteries_V2.Init();

            var builder = WebApplication.CreateBuilder(args);

            // Disable Kestrel headers
            builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

            // Add webapi controllers services
            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents()
                .AddAuthenticationStateSerialization(options =>
                {
                    options.SerializeAllClaims = true;
                });


            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            /*
             * Replaced by the more complex scheme below to support both JWT Bearer and Identity cookies
             * builder.Services.AddAuthentication(options =>
             *   {
             *       options.DefaultScheme = IdentityConstants.ApplicationScheme;
             *       options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
             *   })
             *   .AddIdentityCookies();
            */
            builder.Services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme = "smart";
                    sharedOptions.DefaultChallengeScheme = "smart";
                })
                .AddPolicyScheme("smart", "Authorization Bearer or Identity cookies", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                        if (authHeader?.StartsWith("Bearer ") == true)
                        {
                            return JwtBearerDefaults.AuthenticationScheme;
                        }
                        return IdentityConstants.ApplicationScheme;
                    };
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),
                        RoleClaimType = ClaimTypes.Role
                    };

                })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<AppUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<AppUser>, IdentityNoOpEmailSender>();

            // Add BankBookDbContext and services
            var visualCarnetConnectionString = builder.Configuration["ConnectionStrings:BankBookConnection"] ?? throw new InvalidOperationException("Connection string 'BankBookConnection' not found.");
            builder.Services.AddDbContext<BankBookDbContext>(options =>
                options.UseSqlite(visualCarnetConnectionString));

            // Add Blazorise related services
            builder.Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                    options.ProductToken = builder.Configuration["Blazorise:ProductToken"] ?? throw new InvalidOperationException("Blazorise license token 'ProductToken' not found.");
                })
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();

            // Add Blazr Render state service
            builder.AddBlazrRenderStateServerServices();

            // Add NavBreadcrumb service
            builder.Services.AddScoped<NavBreadcrumbService>();

            // Add HttpClient service
            builder.Services.AddHttpClient();

            // Add app services
            builder.Services.AddTransient<IUserManagementService, UserManagementServerService>();
            builder.Services.AddTransient<ITasklistService, TasklistServerService>();
            builder.Services.AddTransient<IBankAccountService, BankAccountServerService>();


            var app = builder.Build();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto |
                    ForwardedHeaders.XForwardedHost
            });

            // My Middlewares
            app.UseMiddleware<DenyEmptyHost>();
            app.UseMiddleware<ConsoleExceptionLogger>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

            // Not needed with Apache or Nginx in front
            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.MapStaticAssets();

            if (!app.Environment.IsDevelopment())
                app.UsePrecompressedStaticFiles();

            app.MapGet("/account/blazorlogout", (SignInManager<AppUser> SignInManager, HttpContext context) =>
            {
                SignInManager.SignOutAsync().Wait();
                context.Response.Redirect("/");
            });

            app.UseAntiforgery();


            // Map webapi controllers
            app.MapControllers();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            // Manage 404
            app.UseStatusCodePagesWithReExecute("/error/notfound", "?statusCode={0}").UseAntiforgery();

            // Create and seed database on first run
            using IServiceScope scope = app.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            // Try to migrate and seed AppDbContext
            try
            {
                AppDbContext context = services.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
                SeedData.InitializeAsync(services).Wait();
            }
            catch (Exception ex)
            {
                ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error occured creating or seeding C3P1.Net database");
            }

            // Try to migrate and seed BankBookDbContext
            try
            {
                BankBookDbContext context = services.GetRequiredService<BankBookDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error occured creating or migrating BankBook database");
            }

            app.Run();
        }
    }
}
