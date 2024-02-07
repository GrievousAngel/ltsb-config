namespace ConfigAdmin;

public interface IConfigService
{
    ServerConfig GetConfiguration(string configFilePath);
}

public class ConfigService : IConfigService
{
    public ServerConfig GetConfiguration(string configFilePath)
    {
        var serverConfigs = new Dictionary<string, Server>();

        var server = "";
        var keySuffix = "";

        foreach (var line in File.ReadLines(configFilePath))
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
                   Default = serverConfigs["DEFAULTS"],
                   Servers = serverConfigs
                             .Where(sc => sc.Key != "DEFAULTS")
                             .ToDictionary(sc => sc.Key, sc => sc.Value)
               };
    }
}

public class ServerConfig
{
    public Server Default { get; set; }

    public Dictionary<string, Server> Servers { get; set; }
}