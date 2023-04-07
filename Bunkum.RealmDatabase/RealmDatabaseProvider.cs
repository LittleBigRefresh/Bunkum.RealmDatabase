using Bunkum.HttpServer.Database;
using Realms;

namespace Bunkum.RealmDatabase;

public abstract class RealmDatabaseProvider<TContext> : IDatabaseProvider<TContext> where TContext : RealmDatabaseContext, new()
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

    public TContext GetContext()
    {
        this._realmStorage.Value ??= Realm.GetInstance(this._configuration);
        
        TContext context = new();
        context.InitializeContext(this._realmStorage.Value);

        return context;
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