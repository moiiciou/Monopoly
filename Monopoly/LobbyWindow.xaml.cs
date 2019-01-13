using Monopoly.Controller;
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
        }

        private void join_button_Click(object sender, RoutedEventArgs e)
        {
            Board board = Board.GetBoard;
            List<int> players = new List<int>();
            object lockBoard = new object();
            List<Task> tasks = new List<Task>();
            players.Add(PlayerManager.CreatePlayer(board, pseudoTxtBox.Text.PadRight(pseudoTxtBox.Text.Length+(15- pseudoTxtBox.Text.Length),'0'), 10000, 0));

            Connection conn = Connection.GetConnection;
          

        }
    }
}
