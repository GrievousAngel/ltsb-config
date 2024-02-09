using MediatR;
using Microsoft.Extensions.Logging;

namespace ConfigAdmin.Features.Edit;

public static class PostEdit
{
    public class Handler : IRequestHandler<Request>
    {
        private readonly IConfigService configService;
        private readonly ILogger<Handler> logger;

        public Handler(ILogger<Handler> logger, IConfigService configService)
        {
            this.logger = logger;
            this.configService = configService;
        }

        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            var config = configService.Get();

            var server = config.Servers[request.Server.Name];

            server.CookieDomain = request.Server.CookieDomain;
            server.Database = request.Server.Database;
            server.Domain = request.Server.Domain;
            server.IpAddress = request.Server.IpAddress;
            server.ServerName = request.Server.ServerName;
            server.Url = request.Server.Url;

            configService.Save(config);
        }
    }

    public class Request : IRequest
    {
        public Server Server { get; set; }
    }
}