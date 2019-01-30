using Monopoly.Controller;
using Monopoly.Core;
using Monopoly.Model.Board;
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
using System.Windows.Shapes;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour LobbyWindow.xaml
    /// </summary>
    public partial class LobbyWindow : Window
    {
        public LobbyWindow()
        {
            InitializeComponent();

            ip_server_input.Text = Tools.GetLocalIPAddress();
        }

        private void join_button_Click(object sender, RoutedEventArgs e)
        {
            if(pseudoTxtBox.Text.Length < 15)
            {
                PlayerManager.CurrentPlayerName = pseudoTxtBox.Text.PadLeft(15, '0');

                Connection.IpServeur = ip_server_input.Text;
                Connection conn = Connection.GetConnection;
                var GameWindow = new GameWindow();
                GameWindow.Show();
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                System.Windows.MessageBox.Show("Votre pseudo doit être inférieur à 15 charactères !");

            }

        }
    }
}
