namespace AdsTestServer;

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;
using TwinCAT.Ads.TcpRouter;

public class AdsRouterService : BackgroundService
{
    private readonly IConfiguration configuration;
    private readonly ILogger<AdsRouterService> logger;

    public AdsRouterService(IConfiguration configuration, ILogger<AdsRouterService> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancel)
    {
        // var router = new AmsTcpIpRouter(logger, configuration);
        var router = new AmsTcpIpRouter(AmsNetId.LocalHost, 48898, null, 48898, Array.Empty<IPAddress>(), logger);

        // Use this overload to instantiate a Router without support of StaticRoutes.xml and parametrize by code
        // AmsTcpIpRouter router = new AmsTcpIpRouter(new AmsNetId("1.2.3.4.5.6"), AmsTcpIpRouter.DEFAULT_TCP_PORT, _logger);
        // router.AddRoute(...);
        await router.StartAsync(cancel); // Start the router
    }
}
