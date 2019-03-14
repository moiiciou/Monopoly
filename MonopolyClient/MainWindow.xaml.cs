using MonopolyClient.Core;
using MonopolyClient.Core.Network;
using MonopolyClient.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonopolyClient
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClientInfo client = new ClientInfo(pseudo_input.Text, "");
            var t = new Thread(() =>
            {
                AsynchIOClient.StartClient(client);
            });
            t.Start();
            Lobby lobby = new Lobby();
            lobby.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClientInfo client2 = new ClientInfo(pseudo_input.Text, "");
            var t2 = new Thread(() =>
            {
                string dataToSend = Tools.SerializeObject<ClientInfo>(client2);          
                AsynchIOClient.Send(AsynchIOClient.client, dataToSend);
                AsynchIOClient.sendDone.WaitOne();
                AsynchIOClient.Receive(AsynchIOClient.client);
                AsynchIOClient.receiveDone.WaitOne();
            });
            t2.Start();
        }
    }
}
