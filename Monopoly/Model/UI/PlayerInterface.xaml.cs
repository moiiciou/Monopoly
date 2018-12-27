using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour PlayerInterface.xaml
    /// </summary>
    public partial class PlayerInterface : UserControl
    {
        public PlayerInterface(string pseudoPlayer, int moneyPlayer)
        {
            InitializeComponent();
            this.DataContext = new PlayerInfo { PseudoPlayer = pseudoPlayer, MoneyPlayer = moneyPlayer.ToString() + "€" };

        }

        private void ChatBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public class PlayerInfo : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected void NotifyPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
            }

            public string MoneyPlayer { get; set; }
            public string PseudoPlayer { get; set; }


        }
    }
}
