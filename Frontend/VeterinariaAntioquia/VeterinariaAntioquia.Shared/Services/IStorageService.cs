namespace VeterinariaAntioquia.Shared.Services;
using System.Threading.Tasks;
public interface IStorageService
{
    Task SaveItemAsync(string key, string value);
    Task<string> GetItemAsync(string key);
    Task RemoveItemAsync(string key);
}