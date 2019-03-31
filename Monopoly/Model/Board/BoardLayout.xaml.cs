using Monopoly.Controller;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;

namespace Monopoly.Model.Board
{
    /// <summary>
    /// Logique d'interaction pour BoardLayout.xaml
    /// </summary>
    public partial class BoardLayout : Grid
    {
        public BoardLayout()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Packet packet = new Packet();
            packet.Type = "pret";
            packet.Content = "";
            packet.ChatMessage = "";
            string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
            Connection.SendMsg(packetToSend);
            button_pret.Visibility = Visibility.Hidden;
        }
    }
}
