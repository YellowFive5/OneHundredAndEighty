#region Usings

using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;

#endregion

namespace OneHundredAndEightyCore.Tests.Windows.Main
{
    public class NotifyPropertyChangedTester
    {
        public List<string> ChangesInvokes { get; set; }

        public NotifyPropertyChangedTester(INotifyPropertyChanged viewModel)
        {
            ChangesInvokes = new List<string>();
            viewModel.PropertyChanged += viewModel_PropertyChanged;
        }

        private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ChangesInvokes.Add(e.PropertyName);
        }

        public void AssertOnPropertyChangedInvoke(int changeIndex, string expectedPropertyName)
        {
            Assert.IsNotNull(ChangesInvokes, "Changes collection was null.");

            Assert.IsTrue(changeIndex < ChangesInvokes.Count,
                          "Changes collection contains ‘{0}’ items and does not have an element at index ‘{1}’.",
                          ChangesInvokes.Count,
                          changeIndex);

            Assert.AreEqual(expectedPropertyName,
                            ChangesInvokes[changeIndex],
                            "Change at index ‘{0}’ is ‘{1}’ and is not equal to ‘{2}’.",
                            changeIndex,
                            ChangesInvokes[changeIndex],
                            expectedPropertyName);
        }
    }
}