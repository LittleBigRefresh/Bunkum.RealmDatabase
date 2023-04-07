using Bunkum.HttpServer.Database;
using Realms;

namespace Bunkum.RealmDatabase;

public class RealmDatabaseContext : IDatabaseContext
{
    private readonly Realm _realm;

    internal RealmDatabaseContext(Realm realm)
    {
        this._realm = realm;
    }
    
    public void Dispose()
    {
        //NOTE: we dont dispose the realm here, because the same thread may use it again, so we just `Refresh()` it
        this._realm.Refresh();
    }
}