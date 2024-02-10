using MediatR;
using Microsoft.Extensions.Logging;

namespace ConfigAdmin.Features.Edit;

public static class GetEdit
{
    public class Handler : IRequestHandler<Request, Response?>
    {
        private readonly IConfigService configService;
        private readonly ILogger<Handler> logger;

        public Handler(ILogger<Handler> logger, IConfigService configService)
        {
            this.logger = logger;
            this.configService = configService;
        }

        public async Task<Response?> Handle(Request request, CancellationToken cancellationToken)
        {
            var config = configService.Get();

            if (!config.Servers.ContainsKey(request.Name))
            {
                logger.LogWarning("Server with name {ServerName} does not exist", request.Name);
                return null;
            }

            var properties = config.Servers[request.Name];
            if (request.Name != Constants.DEFAULTS)
            {
                // Remove existing properties from the defaults
                // This is used to populate the drop down when adding new properties
                foreach (var key in properties.Keys)
                {
                    config.Defaults.Remove(key);
                }
            }

            return new Response
                   {
                       FileLastModified = config.LastModified,
                       Name = request.Name,
                       Properties = properties,
                       Defaults = config.Defaults
                   };
        }
    }

    public class Request : IRequest<Response?>
    {
        public string Name { get; init; }
    }

    public class Response
    {
        public Dictionary<string, string> Defaults { get; set; }

        public long FileLastModified { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}