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
#pragma warning disable CS0105 // La directive using de 'System.Linq' est apparue précédemment dans cet espace de noms
using System.Linq;
#pragma warning restore CS0105 // La directive using de 'System.Linq' est apparue précédemment dans cet espace de noms
#pragma warning disable CS0105 // La directive using de 'System' est apparue précédemment dans cet espace de noms
using System;
#pragma warning restore CS0105 // La directive using de 'System' est apparue précédemment dans cet espace de noms
using Monopoly.Model.Case;
using Newtonsoft.Json;
using System.Net.Sockets;
using server.Model;

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
            UpdatePseudo(pseudoPlayer);
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
            Console.WriteLine("update property lancer");

            if (player.Properties != null && player.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
            {
             Console.WriteLine("update property lancer");

                foreach (PropertyInfo property in player.Properties)
                {

                        PropertyInfo propertyInfo = (PropertyInfo)property;
                        PropertyCase propertyCase = Core.Tools.GetPropertyByName(propertyInfo.Location);
                        Console.WriteLine("propertyInfo location : " + propertyInfo.Location);
                        Console.WriteLine("propertyCase location : " + propertyCase.CaseInformation.Location);

                        PropertyInfo cardInfo = propertyCase.Card.CardInformation;

                        if (!property_list.Items.Cast<PropertyInfo>().Any(x => x.Location == cardInfo.Location))
                        {
                            property_list.Items.Add(cardInfo);
                            Console.WriteLine("Carte ajouté");

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
