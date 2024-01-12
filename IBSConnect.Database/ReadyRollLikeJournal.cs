using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DbUp.Engine;

namespace IBSConnect.Database;

public class ReadyRollLikeJournal : IJournal
{
    private readonly IJournal _journal;
    private IEnumerable<string> _executedScripts;

    public ReadyRollLikeJournal(IJournal journal)
    {
        _journal = journal;
    }
    public string[] GetExecutedScripts()
    {
        _executedScripts = _journal.GetExecutedScripts();
        return _executedScripts.ToArray();
    }

    public void StoreExecutedScript(SqlScript script, Func<IDbCommand> dbCommandFactory)
    {
        if (!script.Name.StartsWith("IBSConnect.Database.Scripts.Programmable_Objects") && !script.Name.StartsWith("IBSConnect.Database.Scripts.Reference-Data"))
        {
            _journal.StoreExecutedScript(script, dbCommandFactory);
        }
    }

    public void EnsureTableExistsAndIsLatestVersion(Func<IDbCommand> dbCommandFactory)
    {
        _journal.EnsureTableExistsAndIsLatestVersion(dbCommandFactory);
    }
}