using Monopoly.Controller;
using Monopoly.Model.Card;
using server;
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
        public delegate void UpdatePseudoCallback(string pseudo);
        public delegate void AddNewPlayerCallback( PlayerInfo player);
        public delegate void UpdateBalanceCallback(int balance);
        public delegate void UpdateBalanceByPlayerInfoCallback(PlayerInfo player);



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
            money_label.Content = balance.ToString();
        }

        public void UpdateBalanceByPlayerInfo(PlayerInfo player)
        {
            if(player.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
            {
                UpdateBalance(player.Balance);
            }
            else
            {
            foreach(PlayerInfoDisplay infoDisplay in PlayerPanel.Children)
            {
                if(infoDisplay.Pseudo == player.Pseudo)
                {
                        Console.WriteLine(infoDisplay.Pseudo);
                        Console.WriteLine(infoDisplay.Balance);

                        infoDisplay.Balance = player.Balance;
                        Console.WriteLine(infoDisplay.Balance);

                    }
                }
            }

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
