using ConfigAdmin;
using Microsoft.Extensions.Options;
using Shouldly;

namespace UnitTests;

public class ConfigServiceTests
{
    [Fact]
    public void GetConfig()
    {
        var filePath = CreateTestConfigFile();
        var service = new ConfigService(new MockAppSettings(filePath));

        var config = service.Get();

        config.ShouldNotBeNull();

        var defaultConfig = config.Servers[Constants.DEFAULTS];
        defaultConfig.Count.ShouldBe(6);
        defaultConfig["SERVER_NAME"].ShouldBe("MRAPPPOOLPORTL01");
        defaultConfig["URL"].ShouldBe("http://dummy.DOMAIN.COMPANY.COM/Available.html");
        defaultConfig["DB"].ShouldBe("SQL_SERVER");
        defaultConfig["IP_ADDRESS"].ShouldBe("10.200.0.3");
        defaultConfig["DOMAIN"].ShouldBe("MYDOMAIN");
        defaultConfig["COOKIE_DOMAIN"].ShouldBe("dummy.DOMAIN.COMPANY.COM");

        var serverConfig = config.Servers["SRVTST0012"];
        serverConfig["SERVER_NAME"].ShouldBe("MRAPPPOOLPORTL0012");
        serverConfig["IP_ADDRESS"].ShouldBe("10.200.0.101");
    }

    [Fact]
    public void SaveChanges()
    {
        var filePath = CreateTestConfigFile();
        var service = new ConfigService(new MockAppSettings(filePath));

        var config = service.Get();

        var serverConfig = config.Servers["SRVTST0003"];
        serverConfig.Add("DOMAIN", "TEST_DOMAIN");
        serverConfig.Add("DB", "TEST_DB");
        service.Save("SRVTST0003", serverConfig, config.LastModified);

        var updatedConfig = service.Get();
        var updatedServer = updatedConfig.Servers["SRVTST0003"];

        updatedServer["SERVER_NAME"].ShouldBe("MRAPPPOOLPORTL0003");
        updatedServer["IP_ADDRESS"].ShouldBe("10.200.0.100");
        updatedServer["DB"].ShouldBe("TEST_DB");
        updatedServer["DOMAIN"].ShouldBe("TEST_DOMAIN");
    }

    private static string CreateTestConfigFile()
    {
        const string testFilePath = "testConfig.txt";

        if (File.Exists(testFilePath))
        {
            // Clean up any existing file from previous runs
            File.Delete(testFilePath);
        }

        File.Copy("config.txt", testFilePath);
        return testFilePath;
    }

    private class MockAppSettings(string configFilePath) : IOptionsMonitor<AppSettings>
    {
        public AppSettings CurrentValue { get; } = new() { ConfigFilePath = configFilePath };

        public AppSettings Get(string? name)
        {
            throw new NotImplementedException();
        }

        public IDisposable? OnChange(Action<AppSettings, string?> listener)
        {
            throw new NotImplementedException();
        }
    }
}