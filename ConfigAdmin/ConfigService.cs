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
        var serverConfigs = new Dictionary<string, Server>();

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
                serverConfigs[server] = new Server { Name = server };
                continue;
            }

            var segments = line.Split('=');

            var key = segments[0].Replace(keySuffix, "").Trim();
            var value = segments[1].Trim();

            var activeConfig = serverConfigs[server];

            switch (key)
            {
                case "SERVER_NAME":
                    activeConfig.ServerName = value;
                    break;
                case "URL":
                    activeConfig.Url = value;
                    break;
                case "DB":
                    activeConfig.Database = value;
                    break;
                case "IP_ADDRESS":
                    activeConfig.IpAddress = value;
                    break;
                case "DOMAIN":
                    activeConfig.Domain = value;
                    break;
                case "COOKIE_DOMAIN":
                    activeConfig.CookieDomain = value;
                    break;
            }
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
    public Dictionary<string, Server> Servers { get; set; }
}