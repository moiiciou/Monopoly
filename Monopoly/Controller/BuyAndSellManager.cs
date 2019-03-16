using Monopoly.Core;
using Monopoly.Model;
using Monopoly.Model.Case;
using Monopoly.Model.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using server;

namespace Monopoly.Controller
{
    public static class BuyAndSellManager
    {

        public static bool CheckIfBuyable(BaseCase baseCase)
        {
            if(baseCase.GetType().ToString() == "Monopoly.Model.Case.PropertyCase")
            {
                PropertyCase propertyCase = (PropertyCase)baseCase;
                if (propertyCase.CaseInformation.Owner == null)
                    return true;
            }

            if (baseCase.GetType().ToString() == "Monopoly.Model.Case.StationCase")
                return true;

            return false;
        }

        public static void BuyProperty(BaseCase baseCase, Player p)
        {
            if (baseCase.GetType().ToString() == "Monopoly.Model.Case.PropertyCase")
            {
                PropertyCase propertyCase = (PropertyCase)baseCase;
                if(propertyCase.CaseInformation.Owner == null && PlayerManager.CurrentPlayerName.Trim('0') == p.playerInfo.Pseudo )
                {
                            try
                            {

                                Packet packet = new Packet();
                                packet.Type = "buyProperty";

                                packet.Content = JsonConvert.SerializeObject(propertyCase.CaseInformation, Formatting.Indented);

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

        public static void PayRent(BaseCase baseCase, Player player)
        {
            if (baseCase.GetType().ToString() == "Monopoly.Model.Case.PropertyCase")
            {
                PropertyCase propertyCase = (PropertyCase)baseCase;
                if(propertyCase.CaseInformation.Owner != null & propertyCase.CaseInformation.Owner != player.playerInfo.Pseudo & player.playerInfo.Pseudo == PlayerManager.CurrentPlayerName.Trim('0'))
                {
                    MessageBox.Show("Cette propriété appartient à "+ propertyCase.CaseInformation.Owner+"!"+Environment.NewLine+"Vous devez lui payer la somme de "+ propertyCase.CaseInformation.Rent+"€");

                    try
                    {
                        //Envoie les informations au serveur
                        Packet packet = new Packet();
                        packet.Type = "payRent";

                        //Récuperer le joueur owner;
                        PlayerInfo receiver = GameManager.playersList[propertyCase.CaseInformation.Owner];
                        
                        // Payer le loyer
                        player.playerInfo.Balance -= BuyAndSellManager.CalculRent(propertyCase);
                        receiver.Balance += BuyAndSellManager.CalculRent(propertyCase);




                        //Renvoyer la list des joueurs
                        List<PlayerInfo> payload = new List<PlayerInfo>();
                        payload.Add(player.playerInfo);
                        payload.Add(receiver);

                        //renvoie les données au serveur
                        packet.Content = JsonConvert.SerializeObject(payload, Formatting.Indented);
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

        public static int CalculRent(PropertyCase propertyCase)
        {
            int rent = propertyCase.CaseInformation.Rent;

            if(propertyCase.CaseInformation.Owner != null)
            {
                if (propertyCase.CaseInformation.NumberOfHouse == 0 & Core.Tools.GetColorProperty(GameManager.playersList[propertyCase.CaseInformation.Owner], propertyCase) == Core.Tools.GetColorProperty(propertyCase))
                {
                    rent = 2 * propertyCase.CaseInformation.Rent;
                }
            }

            if (propertyCase.CaseInformation.NumberOfHouse == 1)
            {
                rent = propertyCase.Card.CardInformation.RentWith1House;
            }

            if (propertyCase.CaseInformation.NumberOfHouse == 2)
            {
                rent = propertyCase.Card.CardInformation.RentWith2House;

            }

            if (propertyCase.CaseInformation.NumberOfHouse == 3)
            {
                rent = propertyCase.Card.CardInformation.RentWith3House;
            }

            if (propertyCase.CaseInformation.NumberOfHouse == 4)
            {
                rent = propertyCase.Card.CardInformation.RentWith4House;
            }

            if(propertyCase.CaseInformation.HasHostel == true)
            {
                rent = propertyCase.Card.CardInformation.RentWithHotel;
            }

            return rent;
        }

        public static bool CanYouBuild(PropertyCase propertyCase,PlayerInfo player)
        {
            return (Core.Tools.GetColorProperty(propertyCase) == Core.Tools.GetColorProperty(player, propertyCase));

        }
    }
}
