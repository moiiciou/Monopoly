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
using Monopoly.Controller;

namespace Monopoly.Model.Board
{
    /// <summary>
    /// Logique d'interaction pour BoardLayout.xaml
    /// </summary>
    public partial class BoardLayout : Grid
    {
        public delegate void UpdateBoardCallback();
        public BoardLayout()
        {
            InitializeComponent();
        }

        private void lancerPartie(object sender, RoutedEventArgs e)
        {
            Connection.SendMsg("/start");
        }
        private void lancerDe(object sender, RoutedEventArgs e)
        {
            Connection.SendMsg("/a");
        }

        public void UpdateBoard()
        { 
                this.StartGame.IsEnabled = !this.StartGame.IsEnabled;
                this.LancerDes.IsEnabled = !this.LancerDes.IsEnabled;
        }
    }
}
