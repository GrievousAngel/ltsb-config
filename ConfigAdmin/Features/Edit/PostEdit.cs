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
            configService.Save(request.Name, request.Properties);
        }
    }

    public sealed class Request : IRequest
    {
        public string Name { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}