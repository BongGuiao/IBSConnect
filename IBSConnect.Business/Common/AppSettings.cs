namespace IBSConnect.Business.Common;

public class AppSettings
{
    public string Secret { get; set; }
    public string ImagePath { get; set; }
    public string BackupPath { get; set; }
    public int CutoffHours { get; set; }
    public Duration WebTokenExpiry { get; set; }
    public Duration AppTokenExpiry { get; set; }
    public Duration AdminTokenExpiry { get; set; }
}