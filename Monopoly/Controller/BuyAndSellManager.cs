﻿using Monopoly.Core;
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

        public static bool BuyProperty(BaseCase baseCase, Player p)
        {
            if (baseCase.GetType().ToString() == "Monopoly.Model.Case.PropertyCase")
            {
                PropertyCase propertyCase = (PropertyCase)baseCase;
                Console.WriteLine("Owner: "+propertyCase.CaseInformation.Owner);
                if(propertyCase.CaseInformation.Owner == null && PlayerManager.CurrentPlayerName.Trim('0') == p.playerInfo.Pseudo )
                {


                    if (MessageBox.Show("Voulez vous achetez " + propertyCase.CaseInformation.Location + " pour " + propertyCase.CaseInformation.Price + "€", "Acheter", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        //Non
                        return false;
                    }
                    else
                    {
                        if(p.playerInfo.Balance >= propertyCase.CaseInformation.Price)
                        {
                            try
                            {
                                // met à jour le propriétaire de la propriété 
                                propertyCase.CaseInformation.Owner = p.playerInfo.Pseudo;

                                //Envoie les informations au serveur
                                Packet packet = new Packet();
                                packet.Type = "buyProperty";

                                // ajoute la propriété au joueur
                                p.playerInfo.Estates.Add(propertyCase.CaseInformation);
                                p.playerInfo.Balance -= propertyCase.CaseInformation.Price;
                                //renvoie les données au serveur
                                packet.Content = JsonConvert.SerializeObject(p.playerInfo, Formatting.Indented);

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
                            return true;
                        }

                       else
                        {
                            MessageBox.Show("Désolés, vous n'avez pas assez d'argent !");
                        }
                        return false;
                    }

                }
            }
            return false;
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

            if(propertyCase.CaseInformation.NumberOfHouse == 1)
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
