using Monopoly.Controller;
using Monopoly.Model.Card;
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
using Monopoly.Model.Board;

namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour PlayerInterface.xaml
    /// </summary>
    public partial class PlayerInterface : UserControl

    {
        public BoardLayout board;
        public delegate void UpdatePseudoCallback(string pseudo);
        public delegate void AddNewPlayerCallback(server.PlayerInfo player);
        public delegate void UpdateBalanceCallback(int balance);



        public PlayerInterface(string pseudoPlayer, int balance)
        {
            InitializeComponent();
            UpdateBalance(balance);
        }

        private void ChatBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void UpdatePseudo(string pseudo)
        {
            pseudo_label.Content = pseudo.Trim('0');
        }

        public void UpdateBalance(int balance)
        {
            money_label.Content = balance.ToString()+"€";
        }

        public void AddNewPlayer(server.PlayerInfo player)
        {
            if(player.Pseudo != PlayerManager.CurrentPlayerName.Trim('0'))
            {
                PlayerInfoDisplay infoHud = new PlayerInfoDisplay(player.Pseudo, player.Balance);
                PlayerPanel.Children.Add(infoHud);
            }

        }
    }

}
