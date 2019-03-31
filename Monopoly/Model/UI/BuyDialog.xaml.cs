using Monopoly.Controller;
using System.Windows;
using System.Windows.Controls;


namespace Monopoly.Model.UI
{
    /// <summary>
    /// Logique d'interaction pour BuyDialog.xaml
    /// </summary>
    public partial class BuyDialog : UserControl
    {
        private string propertyName;
        private string propertyPrice;
        public string DialogText { get; set; }

        public BuyDialog(string location, string price)
        {
            InitializeComponent();

            propertyName = location;
            propertyPrice = price;

            DialogText = "Voulez vous achetez " + propertyName + " pour " + propertyPrice;

            DataContext = this;
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
