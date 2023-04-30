namespace AdsTestServer;

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;
using TwinCAT.Ads.Server;
using TwinCAT.Ads.Server.TypeSystem;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

public class TestDoubleAdsSymbolicServer : AdsSymbolicServer
{
    private readonly ILogger<TestDoubleAdsSymbolicServer> logger;
    private SymbolicAnyTypeMarshaler symbolMarshaler = new();
    private Dictionary<string, object> memory = new()
    {
        { "MAIN.TestInt", (int)42 },
    };

    public TestDoubleAdsSymbolicServer(ILogger<TestDoubleAdsSymbolicServer> logger)
        : base(25000, nameof(TestDoubleAdsSymbolicServer), logger)
    {
        this.logger = logger;

        var dtDInt = new PrimitiveType("DINT", typeof(int));

        symbolFactory?
            .AddType(dtDInt);

        var general = new DataArea("General", 0x01, 0x1000, 0x1000);

        symbolFactory?
            .AddDataArea(general);

        symbolFactory?
            .AddSymbol("MAIN.TestInt", dtDInt, general);
    }

    protected override AdsErrorCode OnGetValue(ISymbol symbol, out object? value)
    {
        AdsErrorCode result = AdsErrorCode.DeviceSymbolNotFound;
        value = null;

        logger.LogInformation("OnGetValue: {symbolPath}", symbol.InstancePath);

        try
        {
            if (memory.TryGetValue(symbol.InstancePath, out value))
            {
                result = AdsErrorCode.NoError;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to resolve symbol path '{symbolPath}'", symbol.InstancePath);
        }

        return result;
    }

    protected override AdsErrorCode OnReadRawValue(ISymbol symbol, Span<byte> span)
    {
        AdsErrorCode result = OnGetValue(symbol, out object? value);

        if (value != null)
        {
            if (symbolMarshaler.TryMarshal(symbol, value, span, out _))
            {
                result = AdsErrorCode.NoError;
            }
            else
            {
                result = AdsErrorCode.DeviceInvalidSize;
            }
        }

        return result;
    }

    protected override AdsErrorCode OnSetValue(ISymbol symbol, object value, out bool valueChanged)
    {
        throw new NotImplementedException();
    }

    protected override AdsErrorCode OnWriteRawValue(ISymbol symbol, ReadOnlySpan<byte> span)
    {
        throw new NotImplementedException();
    }
}