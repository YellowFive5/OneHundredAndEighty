﻿#region Usings

using System;
using System.Globalization;
using System.IO;
using OneHundredAndEightyCore.Enums;
using OneHundredAndEightyCore.Windows.MessageBox;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class VersionChecker : IVersionChecker
    {
        private readonly DBService dbService;
        private readonly IConfigService configService;
        private readonly IMessageBoxService messageBoxService;

        private const double AppVersion = 2.4;
        private double currentDbVersion;

        public VersionChecker(DBService dbService,
                              IConfigService configService,
                              IMessageBoxService messageBoxService)
        {
            this.dbService = dbService;
            this.configService = configService;
            this.messageBoxService = messageBoxService;
        }

        public void CheckVersions()
        {
            CheckDbExists();

            currentDbVersion = Converter.ToDouble(dbService.SettingsGetValue(SettingsType.DBVersion));

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
                        From2_2to2_3();
                        From2_3to2_4();
                        break;
                    case 2.1:
                        From2_1to2_2();
                        From2_2to2_3();
                        From2_3to2_4();
                        break;
                    case 2.2:
                        From2_2to2_3();
                        From2_3to2_4();
                        break;
                    case 2.3:
                        From2_3to2_4();
                        break;
                    default:
                        messageBoxService.ShowError(Resources.Resources.ErrorDbMigrationText,
                                                    currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                                    AppVersion.ToString("F1", CultureInfo.InvariantCulture));
                        throw new Exception("DB migrating error");
                }
            }
            catch (Exception)
            {
                RevertDb();

                messageBoxService.ShowError(Resources.Resources.ErrorDbMigrationText,
                                            currentDbVersion.ToString("F1", CultureInfo.InvariantCulture),
                                            AppVersion.ToString("F1", CultureInfo.InvariantCulture));

                throw;
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

        private void From2_2to2_3()
        {
            dbService.MigrateFrom2_2to2_3();
        }

        private void From2_3to2_4()
        {
            dbService.MigrateFrom2_3to2_4();
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