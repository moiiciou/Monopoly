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
using System.Linq;
using System;
using Monopoly.Model.Case;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour PlayerInterface.xaml
    /// </summary>
    public partial class PlayerInterface : UserControl

    {
        public delegate void UpdatePseudoCallback(string pseudo);
        public delegate void AddNewPlayerCallback(PlayerInfo player);
        public delegate void UpdateBalanceCallback(int balance);
        public delegate void UpdateBalanceByPlayerInfoCallback(PlayerInfo player);
        public delegate void UpdatePropertyCallback(PlayerInfo player);



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

        public void UpdateProperty(PlayerInfo player)
        {
            if (player.Estates != null && player.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
            {
                foreach (CaseInfo property in player.Estates)
                {
                    PropertyCase propertyCase = Core.Tools.GetPropertyByName(property.Location);
                    Card.CardInfo cardInfo = propertyCase.Card.CardInformation;
                    if (!property_list.Items.Cast<Card.CardInfo>().Any(x => x.TextPropertyName == cardInfo.TextPropertyName))
                    {

                        property_list.Items.Add(cardInfo);

                    }

                }
            }
        }

        public void UpdateBalanceByPlayerInfo(PlayerInfo player)
        {
            if (player.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
            {
                UpdateBalance(player.Balance);
            }
            else
            {
                foreach (PlayerInfoDisplay infoDisplay in PlayerPanel.Children)
                {
                    if (infoDisplay.Pseudo == player.Pseudo)
                    {
                        infoDisplay.Balance = player.Balance;
                    }
                }
            }

        }

        public void AddNewPlayer(PlayerInfo player)
        {
            if (player.Pseudo != PlayerManager.CurrentPlayerName.Trim('0'))
            {
                PlayerInfoDisplay infoHud = new PlayerInfoDisplay(player.Pseudo, player.Balance);
                PlayerPanel.Children.Add(infoHud);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (property_list.SelectedItem != null)
            {
                PropertyCase property = Core.Tools.GetPropertyByName(((CaseInfo)property_list.SelectedValue).Location);
                try
                {

                    Packet packet = new Packet();
                    packet.Type = "buildHouse";

                    packet.Content = JsonConvert.SerializeObject(property.CaseInformation, Formatting.Indented);

                    string message = JsonConvert.SerializeObject(packet, Formatting.Indented);
                    byte[] msg = Encoding.UTF8.GetBytes(Connection.GetConnection.GetSequence() + PlayerManager.CurrentPlayerName + message);
                    int DtSent = Connection.GetConnection.ClientSocket.Send(msg, msg.Length, SocketFlags.None);
                    Console.WriteLine(message);
                    if (DtSent == 0)
                    {
                        MessageBox.Show("Aucune donnée n'a été envoyée");
                    }

                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }



        }

    }
}
