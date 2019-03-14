using MonopolyClient.Controleur;
using MonopolyClient.Core;
using MonopolyClient.Core.Network;
using MonopolyClient.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;


namespace MonopolyClient.UI
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        public Lobby()
        {
            InitializeComponent();
            var t = new Thread(() =>
            {

                    Thread.Sleep(100);
                    ClientMessage clientMessage = new ClientMessage();
                    clientMessage.Command = "getPlayersInfos";
                    string dataToSend = Tools.SerializeObject<ClientMessage>(clientMessage);
                    AsynchIOClient.Send(AsynchIOClient.client, dataToSend);
                    AsynchIOClient.sendDone.WaitOne();
                    AsynchIOClient.Receive(AsynchIOClient.client);
                    AsynchIOClient.receiveDone.WaitOne();

            });
            t.Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
