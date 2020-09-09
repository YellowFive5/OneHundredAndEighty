#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Windows.MessageBox;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class VersionChecker : IVersionChecker
    {
        private readonly Version appVersion;
        private readonly IFileSystemService fileSystemService;
        private readonly IDbService dbService;
        private readonly IConfigService configService;
        private readonly IMessageBoxService messageBoxService;

        private Version currentDbVersion;
        private readonly List<Migration> migrations;


        public VersionChecker(Version appVersion,
                              IFileSystemService fileSystemService,
                              IDbService dbService,
                              IConfigService configService,
                              IMessageBoxService messageBoxService)
        {
            this.appVersion = appVersion;
            this.fileSystemService = fileSystemService;
            this.dbService = dbService;
            this.configService = configService;
            this.messageBoxService = messageBoxService;
            migrations = new List<Migration>
                         {
                             new Migration(new Version(2, 1), dbService.MigrateFrom2_0to2_1),
                             new Migration(new Version(2, 2), dbService.MigrateFrom2_1to2_2),
                             new Migration(new Version(2, 3), dbService.MigrateFrom2_2to2_3),
                             new Migration(new Version(2, 4), dbService.MigrateFrom2_3to2_4),
                         };
        }

        public void CheckAndUpdate()
        {
            
            CheckDbExists();

            currentDbVersion = Converter.ToVersion(dbService.SettingsGetValue(SettingsType.DBVersion));

            if (appVersion > currentDbVersion)
            {
                var confirm = messageBoxService.AskWarningQuestion(Resources.Resources.VersionsMismatchWarningQuestionText,
                                                                   currentDbVersion.ToString(),
                                                                   appVersion.ToString());
                if (confirm)
                {
                    DoMigrations();
                }
                else
                {
                    messageBoxService.ShowError(Resources.Resources.VersionsMismatchErrorText,
                                                appVersion.ToString(),
                                                currentDbVersion.ToString());
                    throw new Exception("DB version and App version is different");
                }
            }
            else if (appVersion < currentDbVersion)
            {
                messageBoxService.ShowError(Resources.Resources.VersionsMismatchDbVersionGreaterErrorText,
                                            currentDbVersion.ToString(),
                                            appVersion.ToString());
                throw new Exception("DB version is greater than App version. This is impossible =)");
            }
        }

        private void CheckDbExists()
        {
            if (!fileSystemService.CheckFileExists(DBService.DatabaseName))
            {
                messageBoxService.ShowError(Resources.Resources.DbNotExistsErrorText, DBService.DatabaseName);
                throw new Exception("DB not exists in root folder");
            }
        }

        private void DoMigrations()
        {
            CreateCopyOfOldDb();

            try
            {
                foreach (var migration in migrations.Where(migration => migration.ToVersion > currentDbVersion))
                {
                    migration.DoMigration();
                }
            }
            catch (Exception)
            {
                RevertDb();

                messageBoxService.ShowError(Resources.Resources.ErrorDbMigrationText,
                                            currentDbVersion.ToString(),
                                            appVersion.ToString());

                throw;
            }

            DeleteCopyOfOldDb();

            messageBoxService.ShowInfo(Resources.Resources.SuccessDbMigrationText,
                                       currentDbVersion.ToString(),
                                       appVersion.ToString());
        }

        private void CreateCopyOfOldDb()
        {
            fileSystemService.CreateFileCopy(DBService.DatabaseName,
                                             DBService.DatabaseCopyName,
                                             true);
        }

        private void RevertDb()
        {
            fileSystemService.CreateFileCopy(DBService.DatabaseCopyName,
                                             DBService.DatabaseName,
                                             true);

            DeleteCopyOfOldDb();
        }

        private void DeleteCopyOfOldDb()
        {
            fileSystemService.DeleteFile(DBService.DatabaseCopyName);
        }
    }
}