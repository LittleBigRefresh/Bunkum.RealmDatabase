using Realms;

namespace Bunkum.RealmDatabase.Test;

public class TestDatabaseProvider : RealmDatabaseProvider<TestDatabaseContext>
{
    protected override ulong SchemaVersion => 1;

    protected override List<Type> SchemaTypes => new()
    {
        typeof(TestModel),
    };

    protected override string Filename => Path.Combine(Path.GetTempPath(), "test");
    protected override void Migrate(Migration migration, ulong oldVersion)
    {
        // none
    }

    protected override bool InMemory => true;
}