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

            var server = config.Servers[request.Name];

            // So we don't get key clashes clear before we repopulate
            server.Clear();
            foreach (var item in request.Properties)
            {
                server.Add(item.Key, item.Value);
            }

            configService.Save(config);
        }
    }

    public sealed class Request : IRequest
    {
        public string Name { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}