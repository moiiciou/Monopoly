using Monopoly.Controller;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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
