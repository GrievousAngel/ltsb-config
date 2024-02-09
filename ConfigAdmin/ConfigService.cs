using System.Text;
using Microsoft.Extensions.Options;

namespace ConfigAdmin;

public interface IConfigService
{
    ServerConfig Get();

    void Save(string name, Dictionary<string, string> properties);
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

    public void Save(string name, Dictionary<string, string> properties)
    {
        var config = Get();

        var server = config.Servers[name];

        // So we don't get key clashes clear before we repopulate
        server.Clear();
        foreach (var item in properties)
        {
            server.Add(item.Key, item.Value);
        }

        // Write back to file
        Persist(config);
    }

    private void Persist(ServerConfig newConfig)
    {
        var contentBuilder = new StringBuilder();
        var iLoop = 0;
        var serverCount = newConfig.Servers.Count;

        foreach (var server in newConfig.Servers)
        {
            // Don't add a key suffix if this is the defaults key
            var keySuffix = server.Key.Equals(Constants.DEFAULTS)
                ? ""
                : string.Concat("{", server.Key, "}");

            contentBuilder.AppendLine($";START {server.Key}");
            foreach (var property in server.Value)
            {
                contentBuilder.AppendLine($"{property.Key}{keySuffix}={property.Value}");
            }

            contentBuilder.AppendLine($";END {server.Key}");
            iLoop++;

            // Don't add an additional line
            if (serverCount > iLoop)
            {
                contentBuilder.AppendLine();
            }
        }

        // TODO: Consider concurrency, has the file changed
        File.WriteAllText(appSettings.ConfigFilePath, contentBuilder.ToString());
    }
}

public sealed class ServerConfig
{
    public Dictionary<string, string> Defaults => Servers[Constants.DEFAULTS];

    public Dictionary<string, Dictionary<string, string>> Servers { get; set; }
}