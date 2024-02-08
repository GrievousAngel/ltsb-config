﻿using MediatR;
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

            // TODO: Pass in file path from appSettings
            var config = configService.Get("../config.txt");

            var servers = new List<Server>();
            servers.AddRange(config.Servers.Values);

            return new Result { Servers = servers };
        }
    }

    public class Request : IRequest<Result>
    {
    }

    public record Result
    {
        public List<Server> Servers { get; init; }
    }
}