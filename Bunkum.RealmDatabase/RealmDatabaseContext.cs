using Bunkum.HttpServer.Database;
using Realms;

namespace Bunkum.RealmDatabase;

public abstract class RealmDatabaseContext : IDatabaseContext
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once InconsistentNaming
    protected Realm _realm { get; private set; } = null!;

    protected RealmDatabaseContext() {}

    internal void InitializeContext(Realm realm)
    {
        this._realm = realm;
    }

    public void Refresh()
    {
        this._realm.Refresh();
    }
    
    public void Dispose()
    {
        //NOTE: we dont dispose the realm here, because the same thread may use it again, so we just `Refresh()` it
        this.Refresh();
    }
}