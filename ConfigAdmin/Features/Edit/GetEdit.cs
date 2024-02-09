using MediatR;
using Microsoft.Extensions.Logging;

namespace ConfigAdmin.Features.Edit;

public static class GetEdit
{
    public class Handler : IRequestHandler<Request, Result>
    {
        private readonly IConfigService configService;
        private readonly ILogger<Handler> logger;

        public Handler(ILogger<Handler> logger, IConfigService configService)
        {
            this.logger = logger;
            this.configService = configService;
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var config = configService.Get();

            config.Servers.TryGetValue(request.Name, out var properties);

            if (request.Name != Constants.DEFAULTS)
            {
                // Remove existing properties from the defaults
                // This is used to populate the drop down when adding new properties
                foreach (var key in properties.Keys)
                {
                    config.Defaults.Remove(key);
                }
            }

            return new Result
                   {
                       Name = request.Name,
                       Properties = properties,
                       Defaults = config.Defaults
                   };
        }
    }

    public class Request : IRequest<Result>
    {
        public string Name { get; init; }
    }

    public record Result
    {
        public Dictionary<string, string> Defaults { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}