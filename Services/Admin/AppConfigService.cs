using C3P1.Net.Data;
using C3P1.Net.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Admin
{
    public class AppConfigService : IAppConfigService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public AppConfigService(AppDbContext context, AuthenticationStateProvider authStateProvider, UserManager<AppUser> userManager)
        {
            _context = context;
            _authStateProvider = authStateProvider;
            _userManager = userManager;
        }

        public async Task<string> GetThemeModeAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null)
            {
                if (user.Identity.IsAuthenticated == true)
                {
                    // authenticated, read theme from db
                    var currentUserId = Guid.Parse(_userManager.GetUserId(user)!);
                    AppConfig? result = await _context.UserAppConfig
                        .Where(x => x.UserId == currentUserId).FirstOrDefaultAsync();

                    if (result is not null && result.ThemeMode is not null)
                    {
                        return result.ThemeMode;
                    }
                    else
                    {
                        // fail to find stored theme, default theme for authenticated users is dark
                        return "dark";
                    }
                }
            }

            // not authenticated, default theme is light
            return "dark";
        }

        public async Task<bool> SetThemeModeAsync(string themeMode)
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null)
            {
                if (user.Identity.IsAuthenticated == true)
                {
                    // authenticated, but is there a stored theme mode ?
                    var currentUserId = Guid.Parse(_userManager.GetUserId(user)!);
                    AppConfig? result = await _context.UserAppConfig
                        .Where(x => x.UserId == currentUserId).FirstOrDefaultAsync();

                    // check if there is a match
                    if (result is not null)
                    {
                        result.ThemeMode = themeMode;
                        _context.Entry(result).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    else
                    {
                        // no match, add an app params entry
                        AppConfig appParam = new AppConfig();
                        appParam.Id = Guid.NewGuid();
                        appParam.UserId = currentUserId;
                        appParam.ThemeMode = themeMode;

                        _context.Add(appParam);
                        await _context.SaveChangesAsync();

                        return true;
                    }

                }
            }

            // not authenticated, failed to set theme mode
            return false;
        }
    }
}
