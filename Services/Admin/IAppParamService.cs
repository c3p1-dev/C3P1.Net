namespace C3P1.Net.Services.Admin
{
    public interface IAppParamService
    {
        public Task<string> GetThemeModeAsync();
        public Task<bool> SetThemeModeAsync(string themeMode);
    }
}
