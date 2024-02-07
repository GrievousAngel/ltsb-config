using MediatR;
using Microsoft.Extensions.Logging;

namespace ConfigAdmin.Features.Index;

public static class GetIndex
{
    public class Handler : IRequestHandler<Request, Result>
    {
        private readonly ILogger<Handler> logger;

        public Handler(ILogger<Handler> logger)
        {
            this.logger = logger;
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            return new Result
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
    }

    public class Request : IRequest<Result>
    {
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