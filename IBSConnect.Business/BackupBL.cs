using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBSConnect.Data;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace IBSConnect.Business
{
    public class BackupBL : IBackupBL
    {
        private readonly IBSConnectDataContext _dataContext;

        public BackupBL(IBSConnectDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Backup(string file)
        {
            using (MySqlConnection conn = new MySqlConnection(_dataContext.Database.GetConnectionString()))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(file);
                        conn.Close();
                    }
                }
            }
        }

        public void Restore(string file)
        {
            using (MySqlConnection conn = new MySqlConnection(_dataContext.Database.GetConnectionString()))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(file);
                        conn.Close();
                    }
                }
            }
        }
    }
}
