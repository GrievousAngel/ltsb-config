using MediatR;
using Microsoft.Extensions.Logging;

namespace ConfigAdmin.Features.Index;

public static class GetIndex
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
            logger.LogDebug("Retrieving list of servers");

            var config = configService.Get();

            return new Result { Servers = config.Servers };
        }
    }

    public class Request : IRequest<Result>
    {
    }

    public record Result
    {
        public Dictionary<string, Dictionary<string, string>> Servers { get; set; }
    }
}