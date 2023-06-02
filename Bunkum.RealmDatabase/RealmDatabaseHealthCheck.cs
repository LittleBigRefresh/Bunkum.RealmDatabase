using System.Diagnostics;
using Bunkum.HttpServer.Database;
using Bunkum.HttpServer.Health;
using static Bunkum.HttpServer.Health.HealthStatusType;

namespace Bunkum.RealmDatabase;

public class RealmDatabaseHealthCheck : IHealthCheck
{
    private readonly IDatabaseProvider<IDatabaseContext> _provider;
    
    public RealmDatabaseHealthCheck(IDatabaseProvider<IDatabaseContext> provider)
    {
        this._provider = provider;
    } 
    
    public HealthStatus RunCheck()
    {
        Stopwatch sw = new();
        sw.Start();

        try
        {
            this._provider.Warmup();
        }
        catch(Exception e)
        {
            return new HealthStatus(Unhealthy, e);
        }

        long ms = sw.ElapsedMilliseconds;
        return new HealthStatus(ms > 100 ? Degraded : Healthy, $"Test took {ms}ms");
    }

    public string Name => "Database";
}