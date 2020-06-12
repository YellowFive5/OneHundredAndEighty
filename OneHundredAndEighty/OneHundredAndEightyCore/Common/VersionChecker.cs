#region Usings

using System;
using System.Globalization;
using System.IO;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class VersionChecker : IVersionChecker
    {
        private readonly DBService dbService;
        private readonly ConfigService configService;
        private readonly MessageBoxService messageBoxService;

        private const double AppVersion = 2.2;
        private double currentDbVersion;

        public VersionChecker(DBService dbService,
                              ConfigService configService,
                              MessageBoxService messageBoxService)
        {
            this.dbService = dbService;
            this.configService = configService;
            this.messageBoxService = messageBoxService;
        }

        public void CheckVersions()
        {
            CheckDbExists();

            currentDbVersion = configService.Read<double>(SettingsType.DBVersion);

            if (AppVersion != currentDbVersion)
            {
                var answer = messageBoxService.AskWarningQuestion(Resources.Resources.VersionsMismatchWarningQuestionText,
                                                                  currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                                                  AppVersion.ToString("F1", CultureInfo.InvariantCulture));
                if (answer)
                {
                    DoMigrations();
                }
                else
                {
                    messageBoxService.ShowError(Resources.Resources.VersionsMismatchErrorText,
                                                AppVersion.ToString("F1", CultureInfo.InvariantCulture),
                                                currentDbVersion.ToString("F1", CultureInfo.InvariantCulture));
                    throw new Exception("DB version and App version is different");
                }
            }
        }

        private void CheckDbExists()
        {
            if (!File.Exists(DBService.DatabaseName))
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
                switch (currentDbVersion)
                {
                    case 2.0:
                        From2_0to2_1();
                        From2_1to2_2();
                        break;
                    case 2.1:
                        From2_1to2_2();
                        break;
                    default:
                        messageBoxService.ShowError(Resources.Resources.ErrorDbMigrationText,
                                                    currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                                    AppVersion.ToString("F1", CultureInfo.InvariantCulture));
                        throw new Exception("DB migrating error");
                }
            }
            catch (Exception e)
            {
                RevertDb();

                messageBoxService.ShowError(Resources.Resources.ErrorDbMigrationText,
                                            currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                            AppVersion.ToString("F1", CultureInfo.InvariantCulture));

                throw e;
            }

            DeleteCopyOfOldDb();

            messageBoxService.ShowInfo(Resources.Resources.SuccessDbMigrationText,
                                       currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                       AppVersion.ToString("F1", CultureInfo.InvariantCulture));
        }

        #region Migrations

        private void From2_0to2_1()
        {
            dbService.MigrateFrom2_0to2_1();
        }

        private void From2_1to2_2()
        {
            dbService.MigrateFrom2_1to2_2();
        }

        #endregion

        #region CRUD

        private void DeleteCopyOfOldDb()
        {
            File.Delete(DBService.DatabaseCopyName);
        }

        private void CreateCopyOfOldDb()
        {
            File.Copy(DBService.DatabaseName,
                      DBService.DatabaseCopyName,
                      true);
        }

        private void RevertDb()
        {
            File.Copy(DBService.DatabaseCopyName,
                      DBService.DatabaseName,
                      true);
            DeleteCopyOfOldDb();
        }

        #endregion
    }
}