using MediatR;
using Microsoft.Extensions.Logging;

namespace ConfigAdmin.Features.Edit;

public static class PostEdit
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IConfigService configService;
        private readonly ILogger<Handler> logger;

        public Handler(ILogger<Handler> logger, IConfigService configService)
        {
            this.logger = logger;
            this.configService = configService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var response = configService.Save(request.Name, request.Properties, request.FileLastModified);

            if ((response.Success == false) && (request.Name != Constants.DEFAULTS))
            {
                var config = configService.Get();
                // It is possible that the defaults have been amended since the edit page was rendered
                // So remove existing properties from the defaults
                foreach (var key in request.Properties.Keys)
                {
                    config.Defaults.Remove(key);
                }

                return new Response
                       {
                           Defaults = config.Defaults,
                           ErrorMessage = response.ErrorMessage,
                           FileLastModified = config.LastModified,
                           Success = response.Success
                       };
            }

            return new Response
                   {
                       Success = response.Success,
                       FileLastModified = response.LastModified,
                       ErrorMessage = response.ErrorMessage
                   };
        }
    }

    public record Request : IRequest<Response>
    {
        public long FileLastModified { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }

    public record Response
    {
        public Dictionary<string, string> Defaults { get; set; }

        public string? ErrorMessage { get; set; }

        public long FileLastModified { get; set; }

        public bool Success { get; set; }
    }
}