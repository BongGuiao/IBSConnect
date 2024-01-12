using DbUp.Builder;
using DbUp.MySql;

namespace IBSConnect.Database;

public static class Extensions
{
    public static UpgradeEngineBuilder ReadyRollLikeJournal(this UpgradeEngineBuilder builder, string schema, string table = "__migrationlog__")
    {
        builder.Configure(c => c.Journal = new ReadyRollLikeJournal(new MySqlTableJournal(() => c.ConnectionManager, () => c.Log, schema, table)));
        return builder;
    }
}