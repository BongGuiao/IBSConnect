using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IBSConnect.Business;
using IBSConnect.Business.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace IBSConnect.AngularApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IBackupBL _backupBl;
        private readonly AppSettings _appSettings;

        public DatabaseController(IBackupBL backupBl, IOptions<AppSettings> appSettings)
        {
            _backupBl = backupBl;
            _appSettings = appSettings.Value;
        }

        [HttpGet("backup")]
        public void Backup()
        {
            var date = DateTime.Now;
            var filename = $"IBSConnect-{date:yyyyMMdd-HHmmss}.sql";
            var path = Path.Join(_appSettings.BackupPath, filename);
            _backupBl.Backup(path);
        }

        [HttpGet("restore")]
        public void Restore(string filename)
        {
            var date = DateTime.Now;
            var backupfilename = $"IBSConnect-Auto-{date:yyyyMMdd-HHmmss}.bak";
            var backuppath = Path.Join(_appSettings.BackupPath, backupfilename);
            _backupBl.Backup(backuppath);

            var path = Path.Join(_appSettings.BackupPath, filename);
            _backupBl.Restore(path);
        }

        [HttpGet]
        public IEnumerable<BackupFile> GetBackups()
        {
            var path = _appSettings.BackupPath;
            var files = Directory.GetFiles(path, "*.sql");

            return files.Select(f =>
            {
                var info = new FileInfo(f);
                return new BackupFile()
                {
                    FileName = info.Name,
                    DateCreated = info.CreationTime,
                    Size = info.Length
                };
            }).OrderByDescending(f => f.DateCreated);
        }

        
    }
}
