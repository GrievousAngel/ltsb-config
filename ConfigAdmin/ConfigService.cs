namespace ConfigAdmin;

public interface IConfigService
{
    ServerConfig GetConfiguration();
}

public class ConfigService : IConfigService
{
    public ServerConfig GetConfiguration()
    {
        // TODO: Read from file

        return new ServerConfig
               {
                   Default = new Server
                             {
                                 CookieDomain = "dummy.DOMAIN.COMPANY.COM",
                                 Database = "example_db",
                                 Domain = "MYDOMAIN",
                                 IpAddress = "10.200.0.3",
                                 Name = "DEFAULTS",
                                 ServerName = "MRAPPPOOLPORTL01",
                                 Url = "http://dummy.DOMAIN.COMPANY.COM/Available.html"
                             },
                   Servers = new Dictionary<string, Server>
                             {
                                 {
                                     "SRVTST0003",
                                     new Server
                                     {
                                         IpAddress = "10.200.0.100",
                                         Name = "SRVTST0003",
                                         ServerName = "SRVTST0003"
                                     }
                                 }
                             }
               };
    }
}

public class ServerConfig
{
    public required Server Default { get; set; }

    public required Dictionary<string, Server> Servers { get; set; }
}