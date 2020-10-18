#region Usings

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OneHundredAndEightyCore.Domain;

#endregion

namespace OneHundredAndEightyCore.Windows.Main
{
    public class DataContext : INotifyPropertyChanged
    {
        public DataContext()
        {
            Players = new List<Player>();
        }

        private List<Player> players;

        public List<Player> Players
        {
            get => players;
            set
            {
                players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}