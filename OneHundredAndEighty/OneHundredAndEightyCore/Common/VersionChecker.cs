#region Usings

using System;
using System.Globalization;
using System.IO;
using System.Windows;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class VersionChecker
    {
        private readonly DBService dbService;
        private readonly ConfigService configService;

        private const double AppVersion = 2.1;
        private double currentDbVersion;

        public VersionChecker(DBService dbService, ConfigService configService)
        {
            this.dbService = dbService;
            this.configService = configService;
        }

        public void CheckVersions()
        {
            currentDbVersion = configService.Read<double>(SettingsType.DBVersion);

            if (AppVersion != currentDbVersion)
            {
                var errorText = string.Format(Resources.VersionsMismatchErrorText,
                                              currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                              AppVersion.ToString("F1", CultureInfo.InvariantCulture));
                var answer = MessageBox.Show(errorText, "Warning", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.Yes)
                {
                    DoMigrations();
                }
                else
                {
                    throw new Exception("DB version and App version is different");
                }
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
                        break;
                    default:
                        throw new Exception("DB migrating error");
                }
            }
            catch (Exception e)
            {
                RevertDb();

                var errorText = string.Format(Resources.ErrorDbMigrationText,
                                              currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                              AppVersion.ToString("F1", CultureInfo.InvariantCulture));
                MessageBox.Show(errorText, "Error", MessageBoxButton.OK);

                throw e;
            }

            DeleteCopyOfOldDb();

            var successText = string.Format(Resources.SuccessDbMigrationText,
                                            currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                            AppVersion.ToString("F1", CultureInfo.InvariantCulture));
            MessageBox.Show(successText, "Success", MessageBoxButton.OK);
        }

        #region Migrations

        private void From2_0to2_1()
        {
            dbService.MigrateFrom2_0to2_1();
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