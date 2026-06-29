namespace VeterinariaAntioquia.Services;
using VeterinariaAntioquia.Shared.Services;
public class SecureStorageService : IStorageService 
{
    public async Task SaveItemAsync(string key, string value)
    {
        await SecureStorage.Default.SetAsync(key, value);
        
    }
    public async Task<string?> GetItemAsync(string key)
    {
        return await SecureStorage.Default.GetAsync(key);
    }

    public async Task RemoveItemAsync(string key)
    {
        SecureStorage.Default.Remove(key);
        await Task.CompletedTask;
    }
}