using MonopolyClient.Core;
using MonopolyClient.Core.Network;
using MonopolyClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyClient.Controleur
{
    class PlayerManager
    {
        // Return all the playerList 
        public static void GetPlayersInfos()
        {
           ClientMessage clientMessage = new ClientMessage();
            clientMessage.Command = "getPlayersInfos";
            clientMessage.Content = "";
            clientMessage.Message = "";
            string packet = Tools.SerializeObject<ClientMessage>(clientMessage);
            AsynchIOClient.Send(AsynchIOClient.client, packet);
            AsynchIOClient.sendDone.WaitOne();
            AsynchIOClient.Receive(AsynchIOClient.client);
            AsynchIOClient.receiveDone.WaitOne();
        }


    }
}
