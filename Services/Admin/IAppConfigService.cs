namespace C3P1.Net.Services.Admin
{
    public interface IAppConfigService
    {
        public Task<string> GetThemeModeAsync();
        public Task<bool> SetThemeModeAsync(string themeMode);
    }
}
