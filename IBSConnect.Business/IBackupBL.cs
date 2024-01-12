namespace IBSConnect.Business;

public interface IBackupBL
{
    void Backup(string file);
    void Restore(string file);
}