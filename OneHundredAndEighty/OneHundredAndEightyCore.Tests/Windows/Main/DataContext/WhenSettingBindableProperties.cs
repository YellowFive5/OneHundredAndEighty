#region Usings

using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using OneHundredAndEightyCore.Domain;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main.DataContext
{
    public class WhenSettingBindableProperties : DataContextTestBase
    {
        private NotifyPropertyChangedTester tester;

        protected override void Setup()
        {
            base.Setup();
            tester = new NotifyPropertyChangedTester(DataContext);
        }

        [Test]
        public void PlayersSetsAndChangeFired()
        {
            var oldValue = DataContext.Players;
            var list = new List<Player>
                       {
                           new Player("Phil", "The Power"),
                           new Player("Michael", "Mighty Mike")
                       };

            DataContext.Players = list;

            oldValue.Should().BeNullOrEmpty();
            DataContext.Players.Should().BeEquivalentTo(list);
            tester.AssertOnPropertyChangedInvoke(0, nameof(DataContext.Players));
        }
    }
}