using Bunkum.HttpServer.Database;
using Realms;

namespace Bunkum.RealmDatabase;

public abstract class RealmDatabaseProvider : IDatabaseProvider<RealmDatabaseContext>
{
    private RealmConfiguration _configuration = null!;

    protected abstract ulong SchemaVersion { get; }
    protected abstract List<Type> SchemaTypes { get; }
    protected abstract string Filename { get; }

    public void Initialize()
    {
        this._configuration = new RealmConfiguration(this.Filename)
        {
            SchemaVersion = this.SchemaVersion,
            Schema = this.SchemaTypes,
            MigrationCallback = this.Migrate,
        };
    }

    protected abstract void Migrate(Migration migration, ulong oldVersion);
    
    private readonly ThreadLocal<Realm> _realmStorage = new(true);

    public RealmDatabaseContext GetContext()
    {
        this._realmStorage.Value ??= Realm.GetInstance(this._configuration);
        return new RealmDatabaseContext(this._realmStorage.Value);
    }
    
    public void Dispose()
    {
        foreach (Realm realmStorageValue in this._realmStorage.Values) 
        {
            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            realmStorageValue?.Dispose();
        }

        this._realmStorage.Dispose();
    }
}