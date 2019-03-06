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
            if (baseCase.GetType().ToString() == "Monopoly.Model.Case.StationCase" | baseCase.GetType().ToString() == "Monopoly.Model.Case.PropertyCase")
                return true;
            return false;
        }

        public static bool BuyProperty(BaseCase baseCase, Player p)
        {
            if (baseCase.GetType().ToString() == "Monopoly.Model.Case.PropertyCase")
            {
                PropertyCase propertyCase = (PropertyCase)baseCase;

                if(propertyCase.CaseInformation.Owner == null && PlayerManager.CurrentPlayerName.Trim('0') == p.NamePlayer )
                {


                    if (MessageBox.Show("Voulez vous achetez " + propertyCase.CaseInformation.Location + " pour " + propertyCase.CaseInformation.Price + "€", "Acheter", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        //Non
                        return false;
                    }
                    else
                    {
                        if(p.Balance >= propertyCase.CaseInformation.Price)
                        {
                            try
                            {
                                propertyCase.CaseInformation.Owner = PlayerManager.CurrentPlayerName.Trim('0');
                                Packet packet = new Packet();
                                packet.Type = "buyProperty";
                                packet.Content = JsonConvert.SerializeObject(propertyCase.DataContext, Formatting.Indented);

                                string message = JsonConvert.SerializeObject(packet, Formatting.Indented);
                                byte[] msg = System.Text.Encoding.UTF8.GetBytes(Connection.GetConnection.GetSequence() + PlayerManager.CurrentPlayerName + message);
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
                            return true;
                        }

                       else
                        {
                            MessageBox.Show("Désolés, vous n'avez pas assez d'argent !");
                        }
                        return false;
                    }

                }
                if (propertyCase.CaseInformation.Owner != null)
                {
                    MessageBox.Show("Cette case appartient à "+ propertyCase.CaseInformation.Owner + Environment.NewLine + "Vous arretez içi vous coutera "+ propertyCase.Card.CardInformation.TextRentValue +"€" );
                }


            }
            return false;
        }
    }
}
