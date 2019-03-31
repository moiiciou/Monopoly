using Monopoly.Controller;
using Monopoly.Model.Case;
using Newtonsoft.Json;
using server;
using server.Model;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
        public delegate void UpdateAvatarCallback(PlayerInfo playerInfo);

        public string ImagePath { get; set; }

        public PlayerInterface(string pseudoPlayer, int balance)
        {
            InitializeComponent();
            UpdateBalance(balance);
            UpdatePseudo(pseudoPlayer);
        }

        private void ChatBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void UpdatePseudo(string pseudo)
        {
            pseudo_label.Content = pseudo.Trim('0');
        }

        public void UpdateAvatar(PlayerInfo playerInfo)
        {
            if(playerInfo.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
            {
                ImagePath = "/Monopoly;component/ressources/templates/default/avatar/" + playerInfo.ColorCode.Trim('#') + ".png";
                DataContext = this;
            }
        }

        public void UpdateBalance(int balance)
        {
            money_label.Content = balance.ToString();
        }

        public void UpdateProperty(PlayerInfo player)
        {
            if (player.Properties != null && player.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
            {
                foreach (PropertyInfo property in player.Properties)
                {

                        PropertyInfo propertyInfo = (PropertyInfo)property;
                        PropertyCase propertyCase = Core.Tools.GetPropertyByName(propertyInfo.Location);
                        PropertyInfo cardInfo = propertyCase.Card.CardInformation;

                        if (!property_list.Items.Cast<PropertyInfo>().Any(x => x.Location == cardInfo.Location))
                        {
                            property_list.Items.Add(cardInfo);

                        }                    
                }
            }

            if(player.Stations !=null && player.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
            {
                foreach(StationInfo stationInfo in player.Stations)
                {
                    if (!stations_list.Items.Cast<StationInfo>().Any(x => x.TextLabel == stationInfo.TextLabel))
                    {
                        stations_list.Items.Add(stationInfo);
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
                PlayerInfoDisplay infoHud = new PlayerInfoDisplay(player.Pseudo, player.Balance, player.ColorCode);
                PlayerPanel.Children.Add(infoHud);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (property_list.SelectedItem != null)
            {
                PropertyCase property = Core.Tools.GetPropertyByName(((PropertyInfo)property_list.SelectedValue).Location);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PropertyCase property = Core.Tools.GetPropertyByName(((PropertyInfo)property_list.SelectedValue).Location);
            try
            {

                Packet packet = new Packet();
                packet.Type = "sellProperty";
                packet.Content = JsonConvert.SerializeObject(property.CaseInformation, Formatting.Indented);

                string message = JsonConvert.SerializeObject(packet, Formatting.Indented);
                byte[] msg = Encoding.UTF8.GetBytes(Connection.GetConnection.GetSequence() + PlayerManager.CurrentPlayerName + message);
                int DtSent = Connection.GetConnection.ClientSocket.Send(msg, msg.Length, SocketFlags.None);
                if (DtSent == 0)
                {
                    MessageBox.Show("Aucune donnée n'a été envoyée");
                }

            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
            property_list.Items.RemoveAt(property_list.Items.IndexOf(property_list.SelectedItem));

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Packet packet = new Packet();
            packet.Type = "useFreeFromJailCard";
            packet.Content = "freeFromJailChance";
            packet.ChatMessage = "";
            string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
            Connection.SendMsg(packetToSend);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Packet packet = new Packet();
            packet.Type = "useFreeFromJailCard";
            packet.Content = "freeFromJailCommunity";
            packet.ChatMessage = "";
            string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
            Connection.SendMsg(packetToSend);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (property_list.SelectedItem != null)
            {
                PropertyCase property = Core.Tools.GetPropertyByName(((PropertyInfo)property_list.SelectedValue).Location);
                Packet packet = new Packet();
                packet.Type = "mortGageProperty";
                packet.Content = JsonConvert.SerializeObject(property.CaseInformation, Formatting.Indented);
                packet.ChatMessage = "";
                string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                Connection.SendMsg(packetToSend);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (stations_list.SelectedItem != null)
            {
                StationInfo stationInfo = (StationInfo)stations_list.SelectedItem;
                Packet packet = new Packet();
                packet.Type = "sellStation";
                packet.Content = JsonConvert.SerializeObject(stationInfo, Formatting.Indented);
                packet.ChatMessage = "";
                string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                Connection.SendMsg(packetToSend);
                stations_list.Items.RemoveAt(stations_list.Items.IndexOf(stations_list.SelectedItem));
            }

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (stations_list.SelectedItem != null)
            {
                StationInfo stationInfo = (StationInfo)stations_list.SelectedItem;
                Packet packet = new Packet();
                packet.Type = "mortGageStation";
                packet.Content = JsonConvert.SerializeObject(stationInfo, Formatting.Indented);
                packet.ChatMessage = "";
                string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                Connection.SendMsg(packetToSend);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (property_list.SelectedItem != null)
            {
                PropertyCase property = Core.Tools.GetPropertyByName(((PropertyInfo)property_list.SelectedValue).Location);
                Packet packet = new Packet();
                packet.Type = "sellHouse";
                packet.Content = JsonConvert.SerializeObject(property.CaseInformation, Formatting.Indented);
                packet.ChatMessage = "";
                string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                Connection.SendMsg(packetToSend);
            }
        }
    }
    
}
