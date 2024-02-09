using Microsoft.Extensions.Options;

namespace ConfigAdmin;

public interface IConfigService
{
    ServerConfig Get();

    void Save(ServerConfig config);
}

public sealed class ConfigService : IConfigService
{
    private readonly AppSettings appSettings;

    public ConfigService(IOptionsMonitor<AppSettings> appSettingOptions)
    {
        appSettings = appSettingOptions.CurrentValue;
    }

    public ServerConfig Get()
    {
        var serverConfigs = new Dictionary<string, Dictionary<string, string>>();

        var server = "";
        var keySuffix = "";

        foreach (var line in File.ReadLines(appSettings.ConfigFilePath))
        {
            if (line.StartsWith(";END") || string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith(";START"))
            {
                server = line.Substring(7, line.Length - 7);
                keySuffix = "{" + server + "}";
                serverConfigs[server] = new Dictionary<string, string>();
                continue;
            }

            var segments = line.Split('=');

            var key = segments[0].Replace(keySuffix, "").Trim();
            var value = segments[1].Trim();

            var activeConfig = serverConfigs[server];

            activeConfig.Add(key, value);
        }

        return new ServerConfig
               {
                   Servers = serverConfigs.ToDictionary(sc => sc.Key, sc => sc.Value)
               };
    }

    public void Save(ServerConfig config)
    {
        // TODO: Persist to file
    }
}

public sealed class ServerConfig
{
    public Dictionary<string, Dictionary<string, string>> Servers { get; set; }
}