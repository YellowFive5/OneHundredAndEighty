#region Usings

using System;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class Migration
    {
        public Version ToVersion { get; }
        private readonly Action migrationAction;

        public Migration(Version toVersion, Action migrationAction)
        {
            this.migrationAction = migrationAction;
            ToVersion = toVersion;
        }

        public void DoMigration()
        {
            migrationAction();
        }
    }
}