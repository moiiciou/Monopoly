using Monopoly.Controller;
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

namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour BuyDialog.xaml
    /// </summary>
    public partial class BuyDialog : UserControl
    {
        public BuyDialog()
        {
            InitializeComponent();
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            BuyAndSellManager.BuyProperty(Board.Board.GetBoard.CasesList[PlayerManager.GetPlayerByPseuso(PlayerManager.CurrentPlayerName.Trim('0')).Position % 40], PlayerManager.SearchPlayer(PlayerManager.CurrentPlayerName.Trim('0')));
            ((Panel)this.Parent).Children.Remove(this);

        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            ((Panel)this.Parent).Children.Remove(this);
        }
    }
}
