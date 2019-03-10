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
        public delegate void AddNewPlayerCallback( PlayerInfo player);
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
            if(player.Estates != null && player.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
            {
                foreach (CaseInfo property in player.Estates)
                {
                    ListBoxItem item = new ListBoxItem { Content = property.Location, Background = (SolidColorBrush)new BrushConverter().ConvertFromString(property.Color) };
                    if (!property_list.Items.Cast<ListBoxItem>().Any(x => x.Content.ToString() == item.Content.ToString()))
                    {
                       
                      property_list.Items.Add(item);

                    }

                }
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (property_list.SelectedItem != null)
                if (MessageBox.Show("Souhaitez vous construire sur la propriété " + ((ListBoxItem)property_list.SelectedValue).Content.ToString(), "Construire", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                //Non
                }
                else
                {
                    PlayerInfo player = GameManager.playersList[PlayerManager.CurrentPlayerName.Trim('0')];
                    PropertyCase property = Core.Tools.GetPropertyByName(((ListBoxItem)property_list.SelectedValue).Content.ToString());

                    if (BuyAndSellManager.CanYouBuild(property, player))
                    {
                        if (property.CaseInformation.NumberOfHouse < 4)
                        {
                            if (MessageBox.Show("Constuire une maison supplémentaire vous coûtera " + property.Card.CardInformation.HouseCost.ToString(), "Construire", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            {
                                //Non
                            }
                            else
                            {
                                if(player.Balance < property.Card.CardInformation.HouseCost)
                                {
                                    MessageBox.Show("Désolés vous n'avez pas assez d'argent !");

                                }
                                else
                                {
                                    try
                                    {
                                        // met à jour le nombre de maison de la propriété 
                                        CaseInfo estate = player.Estates.FirstOrDefault(x => x.Location == property.CaseInformation.Location);
                                        if (estate != null)
                                            estate.NumberOfHouse++;

                                       player.Balance -= property.Card.CardInformation.HouseCost;

                                        //Envoie les informations au serveur
                                        Packet packet = new Packet();
                                        packet.Type = "buyProperty";

                                        // ajoute la propriété au joueur
                                        //renvoie les données au serveur
                                        packet.Content = JsonConvert.SerializeObject(player, Formatting.Indented);

                                        string message = JsonConvert.SerializeObject(packet, Formatting.Indented);
                                        byte[] msg = Encoding.UTF8.GetBytes(Connection.GetConnection.GetSequence() + PlayerManager.CurrentPlayerName + message);
                                        int DtSent = Connection.GetConnection.ClientSocket.Send(msg, msg.Length, SocketFlags.None);
                                        Console.WriteLine(message);

                                        property.UpdateBackground();

                                        if (DtSent == 0)
                                        {
                                            MessageBox.Show("Aucune donnée n'a été envoyée");
                                        }

                                    }
                                    catch (Exception E)
                                    {
                                        MessageBox.Show(E.Message);
                                    }

                                    MessageBox.Show("Maison construite !");
                                }
                            }
                        }

                        if (property.CaseInformation.NumberOfHouse == 4 & property.CaseInformation.HasHostel == false )
                        {
                            if (MessageBox.Show("Constuire un hotel vous coûtera " + property.Card.CardInformation.HostelCost.ToString(), "Construire", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            {
                                //Non
                            }
                            else
                            {
                                if (player.Balance < property.Card.CardInformation.HostelCost)
                                {
                                    MessageBox.Show("Désolés vous n'avez pas assez d'argent !");

                                }
                                else
                                {
                                    try
                                    {
                                        // met à jour le nombre de maison de la propriété 
                                        CaseInfo estate = player.Estates.FirstOrDefault(x => x.Location == property.CaseInformation.Location);
                                        if (estate != null)
                                            estate.HasHostel = true;

                                        player.Balance -= property.Card.CardInformation.HostelCost;

                                        //Envoie les informations au serveur
                                        Packet packet = new Packet();
                                        packet.Type = "buyProperty";

                                        // ajoute la propriété au joueur
                                        //renvoie les données au serveur
                                        packet.Content = JsonConvert.SerializeObject(player, Formatting.Indented);

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
                                    MessageBox.Show("Hotel construit !");

                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vous devez posséder tout les terrains de même couleur avant de pouvoir construire dessus !");

                    }

                }
                    
            else
                MessageBox.Show("Selectionner une propriété dans la liste !");
        }
    }

}
