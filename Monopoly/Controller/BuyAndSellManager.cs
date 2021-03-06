﻿using Monopoly.Model;
using Monopoly.Model.Case;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

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
            {
                StationCase stationCase = (StationCase)baseCase;
                if (stationCase.CaseInformation.Owner == null)
                    return true;

            }
            if (baseCase.GetType() == typeof(CompanyCase))
            {
                CompanyCase companyCase = (CompanyCase)baseCase;
                if (companyCase.ComInfo.Owner == null)
                    return true;

            }

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

            if (baseCase.GetType() == typeof(StationCase))
            {
                StationCase stationCase = (StationCase)baseCase;
                if (stationCase.CaseInformation.Owner == null && PlayerManager.CurrentPlayerName.Trim('0') == p.playerInfo.Pseudo)
                {
                    try
                    {

                        Packet packet = new Packet();
                        packet.Type = "buyStation";

                        packet.Content = JsonConvert.SerializeObject(stationCase.CaseInformation, Formatting.Indented);

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

            if (baseCase.GetType() == typeof(CompanyCase))
            {
                CompanyCase companyCase = (CompanyCase)baseCase;
                if (companyCase.ComInfo.Owner == null && PlayerManager.CurrentPlayerName.Trim('0') == p.playerInfo.Pseudo)
                {
                    try
                    {

                        Packet packet = new Packet();
                        packet.Type = "buyCompany";
                        packet.Content = JsonConvert.SerializeObject(companyCase.ComInfo, Formatting.Indented);
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

    }
}
