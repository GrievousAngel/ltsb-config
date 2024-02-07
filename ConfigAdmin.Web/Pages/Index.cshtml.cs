using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConfigAdminWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        this.logger = logger;
    }

    public Result Data { get; private set; }

    public void OnGet()
    {
        logger.LogDebug("Getting Servers");

        Data = new Result
               {
                   Servers =
                   [
                       new Result.Server
                       {
                           CookieDomain = "dummy.DOMAIN.COMPANY.COM",
                           Database = "example_db",
                           Domain = "MYDOMAIN",
                           IpAddress = "10.200.0.3",
                           Name = "DEFAULTS",
                           ServerName = "MRAPPPOOLPORTL01",
                           Url = "http://dummy.DOMAIN.COMPANY.COM/Available.html"
                       }
                   ]
               };
    }

    public record Result
    {
        public List<Server> Servers { get; init; }

        public record Server
        {
            public string CookieDomain { get; init; }

            public string Database { get; init; }

            public string Domain { get; init; }

            public string IpAddress { get; init; }

            public string Name { get; init; }

            public string ServerName { get; init; }

            public string Url { get; init; }
        }
    }
}