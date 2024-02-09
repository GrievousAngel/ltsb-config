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

            config.Servers.TryGetValue(request.Name, out var server);

            return new Result { Name = request.Name, Properties = server };
        }
    }

    public class Request : IRequest<Result>
    {
        public string Name { get; init; }
    }

    public record Result
    {
        public string Name { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}