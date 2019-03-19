using Monopoly.Model;
using Monopoly.Model.Board;
using Monopoly.Model.Card;
using Newtonsoft.Json;
using server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Monopoly.Controller
{
    class CardManager
    {
        public static void drawCard(PlayerInfo playerInfo, string type)
        {
            try
            {
                Packet packet = new Packet();

                if(type =="chance")
                {
                    packet.Type = "drawChance";

                }
                if(type =="community")
                {
                    packet.Type = "drawCommunity";
                }

                packet.Content = JsonConvert.SerializeObject(playerInfo, Formatting.Indented);
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
