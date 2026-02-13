using System;
using System.IO;
using System.Threading.Tasks;

namespace AvaloniaApplication2.Services;

public class AuthService
{
    private readonly string _path;

    public AuthService()
    {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClientDemo");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "token.txt");
    }

    public Task SaveTokenAsync(string token)
    {
        File.WriteAllText(_path, token ?? "");
        return Task.CompletedTask;
    }

    public Task<string?> GetTokenAsync()
    {
        if (!File.Exists(_path)) return Task.FromResult<string?>(null);
        var t = File.ReadAllText(_path).Trim();
        return Task.FromResult<string?>(string.IsNullOrWhiteSpace(t) ? null : t);
    }

    public Task ClearTokenAsync()
    {
        if (File.Exists(_path)) File.Delete(_path);
        return Task.CompletedTask;
    }
}