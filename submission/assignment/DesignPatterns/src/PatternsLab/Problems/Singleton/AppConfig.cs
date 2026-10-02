namespace PatternsLab.Problems.Singleton;

/// <summary>
/// Singleton: there is exactly ONE AppConfig in the whole application.
/// - sealed + private constructor  -> nobody can write "new AppConfig()" (R5)
/// - Lazy&lt;T&gt; creates it on first use, only once, and is thread-safe by default (R3, R4)
/// - everybody reads AppConfig.Instance, so they all see the same values (R1, R2)
/// </summary>
public sealed class AppConfig
{
    private static readonly Lazy<AppConfig> _instance = new(() => new AppConfig());

    public static AppConfig Instance => _instance.Value;

    private static int _loadCount;
    public static int LoadCount => _loadCount;

    public string DbConnection { get; set; }
    public string Theme { get; set; }

    private AppConfig()
    {
        Interlocked.Increment(ref _loadCount);
        Console.WriteLine($"[AppConfig] Loading settings from disk... (load #{LoadCount})");
        Thread.Sleep(300);
        DbConnection = "Server=localhost;Db=School";
        Theme = "Light";
    }
}

public class DatabaseService
{
    public AppConfig Config => AppConfig.Instance;

    public void Connect() => Console.WriteLine($"Connecting to {Config.DbConnection}");
}

public class UiService
{
    public AppConfig Config => AppConfig.Instance;

    public void Render() => Console.WriteLine($"UI is using theme: {Config.Theme}");
}
