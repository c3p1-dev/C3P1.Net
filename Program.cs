using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using C3P1.Net.Data;
using C3P1.Net.Identity.Data;
using C3P1.Net.Services.Admin;
using C3P1.Net.Services.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("C3P1ContextConnection");
var configuration = builder.Configuration;

// Add DataContext on the main database
builder.Services.AddDbContext<C3P1Context>(options =>
    options.UseSqlite(connectionString)); ;

// Add Identity support
builder.Services.AddDefaultIdentity<C3P1User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<C3P1Context>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

// Add API Token authentication support
// Adding Authentication
/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidAudience = configuration["JWT:ValidAudience"],
         ValidIssuer = configuration["JWT:ValidIssuer"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
     };
 });*/

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Blazorise support
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

// Register services
builder.Services.AddTransient<IUsersAdminService, UsersAdminService>();
builder.Services.AddTransient<ITasklistService, TasklistService>();

var app = builder.Build();

// Configure the HTTP request pipelineand Exceptions following
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// HTTPS redirection is forced by Apache
//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

// Needed for Identity
app.UseAuthentication();
app.UseAuthorization();

// Map routes for API Controllers, and Blazor
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Create and seed database on first run
using IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;
try
{
    // check if CardioContext Database exists and create it if not
    C3P1Context context = services.GetRequiredService<C3P1Context>();
    //context.Database.EnsureCreated();
    context.Database.Migrate();

    // Seed IdentityContext Database with basic datas
    SeedDatabase.InitializeAsync(services).Wait();
}
catch (Exception ex)
{
    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error occured creating or seeding the database.");
}

app.Run();
