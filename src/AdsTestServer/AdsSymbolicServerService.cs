namespace AdsTestServer;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads.Server;

public class AdsSymbolicServerService : BackgroundService
{
    private readonly AdsSymbolicServer server;
    private readonly ILogger<AdsSymbolicServerService> logger;

    public AdsSymbolicServerService(AdsSymbolicServer server, ILogger<AdsSymbolicServerService> logger)
    {
        this.server = server;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await server.ConnectServerAndWaitAsync(stoppingToken);
    }
}