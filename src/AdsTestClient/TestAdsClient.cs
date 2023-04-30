namespace AdsTestClient;

using TwinCAT.Ads;

public class TestAdsClient
{
    private readonly ILogger<TestAdsClient> logger;
    private AdsClient client = new();

    public TestAdsClient(ILogger<TestAdsClient> logger)
    {
        this.logger = logger;
    }

    public void Connect()
    {
        client.Connect(AmsNetId.Local, 25000);
    }

    public T Read<T>(string symbolPath)
        where T : struct
    {
        T result = default;
        uint handle = 0;
        try
        {
            handle = client.CreateVariableHandle(symbolPath);
            result = (T)client.ReadAny(handle, typeof(T));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error reading {symbolPath}: {errorMessage}", symbolPath, e.Message);
        }
        finally
        {
            client.DeleteVariableHandle(handle);
        }

        return result;
    }
}
