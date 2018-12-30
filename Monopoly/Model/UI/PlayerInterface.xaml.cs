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
        public PlayerInterface(Player player)
        {
            InitializeComponent();
            this.DataContext = player.playerInfo;

        }

        private void ChatBox_Loaded(object sender, RoutedEventArgs e)
        {

        }


    }

    public partial class PlayerInfo : INotifyPropertyChanged
    {
        private string _name = "ERROR";
        private string _balance = "ERROR";
        public event PropertyChangedEventHandler PropertyChanged;

        public PlayerInfo(string name, string balance)
        {
            PseudoPlayer = name;
            MoneyPlayer = balance;
        }
        public string PseudoPlayer
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("PseudoPlayer");
            }
        }
        public string MoneyPlayer
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged("MoneyPlayer");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
                handler(this, new PropertyChangedEventArgs(_balance));

            }
        }
    }
}
